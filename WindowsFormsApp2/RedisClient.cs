using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Monitor;
using Azure.ResourceManager.Monitor.Models;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.Redis.Models;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager.RedisEnterprise.Models;
using Pipelines.Sockets.Unofficial.Arenas;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RetentionPolicy = Azure.ResourceManager.Monitor.Models.RetentionPolicy;

namespace WindowsFormsApp2
{
    public class RedisClient
    {
        public static async Task<RedisResource> CreateRedisResource(string CacheName, RedisCacheOptions options,
            RedisCollection RedisCollection, bool forceCreateReplicas = false)
        {
            // TODO: Handle the caches in provision failed state
            var isCloudService = CacheFeaturesHelper.IsCloudService(options.CacheFeatures);

            var cacheName = CacheName;

            RedisCreateOrUpdateContent createParams = CreateParametersFromOptions(options, forceCreateReplicas);

            RedisResource cache = (await RedisCollection.CreateOrUpdateAsync(WaitUntil.Completed, cacheName, createParams)).Value;
            return cache;
        }

        public static async Task<RedisEnterpriseClusterResource> CreateRedisEnterpriseCacheAsync(AzureClient azureClient, string CacheName, EnterpriseCacheOptions options,
           RedisEnterpriseClusterCollection RedisEC)
        {
            // Approximately. I know 63 is too long because with redis enterprise caches max
            // length is actually limited by an additional suffix of {.regionname}, which can
            // be quite long e.g. (hypothetically, or not) australiasoutheast, dodsouthcentralus,
            // chinanortheast2, germanywestcentral, southafricanortheast
            int MAX_SAFE_NAME_LENGTH = 42;
            var cacheName = CacheName;

            var clusterParams = new RedisEnterpriseClusterData(options.RegionName, new RedisEnterpriseSku(options.SkuName)); ;

            var createdCache = RedisEC.CreateOrUpdate(WaitUntil.Completed, cacheName, clusterParams).Value;
            //var createdCache = await managementClient.RedisEnterprise.CreateAsync(ResourceGroupName, cacheName, clusterParams);

            var databaseParams = GenerateDatabaseParams(azureClient.ArmClient, options);
            //var databaseCreate = await managementClient.Databases.CreateAsync(ResourceGroupName, cacheName, "default", databaseParams);
            var databaseCreate = createdCache.GetRedisEnterpriseDatabases().CreateOrUpdate(WaitUntil.Completed, "default", databaseParams);

            return createdCache;

        }

        public static async Task DeleteRedisEnterpriseCacheAsync(string clusterName,
            RedisEnterpriseClusterCollection RedisEnterprise
            )
        {
            await RedisEnterprise.Get(clusterName).Value.DeleteAsync(WaitUntil.Completed);
        }

        public static async Task<RedisEnterpriseClusterResource> ScaleRedisEnterpriseCacheAsync(string clusterName, RedisEnterpriseSkuName targetSkuName, int? targetCapacity, RedisEnterpriseClusterCollection RedisEnterprise)
        {
            var clusterData = (await RedisEnterprise.GetAsync(clusterName)).Value.Data;
            clusterData.Sku = new RedisEnterpriseSku(targetSkuName) { Capacity = targetCapacity };
            return (await RedisEnterprise.CreateOrUpdateAsync(WaitUntil.Completed, clusterName, clusterData)).Value;
        }

        public static async Task CreateDiagnosticLogSettingRedisEnterpriseCacheAsync(string category, string storageuri, string cacheuri, RedisEnterpriseClusterCollection RedisEnterprise)
        {
            // get your azure access token, for more details of how Azure SDK get your access token, please refer to https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication?tabs=command-line
            TokenCredential cred = new DefaultAzureCredential();
            // authenticate your client
            ArmClient client = new ArmClient(cred);
            string resourceUri = cacheuri;
            string name = "mysetting";
            ResourceIdentifier diagnosticSettingResourceId = DiagnosticSettingResource.CreateResourceIdentifier(resourceUri, name);
            DiagnosticSettingResource diagnosticSetting = client.GetDiagnosticSettingResource(diagnosticSettingResourceId);

            // invoke the operation
            DiagnosticSettingData data = new DiagnosticSettingData()
            {
                StorageAccountId = new ResourceIdentifier(storageuri),
                Logs =
                {
                new LogSettings(true)
                {
                Category = category,
                RetentionPolicy = new RetentionPolicy(false,0),
                }
                }
            };
            ArmOperation<DiagnosticSettingResource> lro = await diagnosticSetting.UpdateAsync(WaitUntil.Completed, data);
            DiagnosticSettingResource result = lro.Value;

            DiagnosticSettingData resourceData = result.Data;
        }

        
        private static RedisCreateOrUpdateContent CreateParametersFromOptions(RedisCacheOptions options, bool forceCreateReplicas = false)
        {
            var isCloudService = CacheFeaturesHelper.IsCloudService(options.CacheFeatures);
            RedisSku? skuobj = null;
            int skucapacity = 0;
            if (options.SkuCapacity > 0)
            {
                skucapacity = options.SkuCapacity;
            }

            switch (options.SkuName.ToString().ToLower())
            {
                case "basic":
                    skuobj = new RedisSku(new RedisSkuName("Basic"), new RedisSkuFamily("C"), skucapacity);
                    break;

                case "standard":
                    skuobj = new RedisSku(new RedisSkuName("Standard"), new RedisSkuFamily("C"), skucapacity);
                    break;

                case "premium":
                    skuobj = new RedisSku(new RedisSkuName("Premium"), new RedisSkuFamily("P"), skucapacity);
                    break;

                default:
                    throw new NotSupportedException($"Unsupported sku: {options.SkuName}");
            }

            var location = new AzureLocation(options.RegionName);
            //Identity need re

            var parameters = new RedisCreateOrUpdateContent(location, skuobj)
            {

                SubnetId = options.SubnetId,
                EnableNonSslPort = options.NonSSL.Equals(true) && options.NonSSL,
                RedisConfiguration = new RedisCommonConfiguration()

            };
            if (options.redisCommonConfiguration != null)
            {
                parameters.RedisConfiguration = options.redisCommonConfiguration;
            }

            if (isCloudService)
            {
                parameters.RedisConfiguration.AdditionalProperties.Add("CacheVmType", BinaryData.FromString("\"CloudService\""));
            }
            else
            {
                parameters.RedisConfiguration.AdditionalProperties.Add("CacheVmType", BinaryData.FromString("\"IaaS\""));
            }


            if (skuobj.Name.ToString() == "Premium" && options.MinShards.HasValue && options.Cluster.HasValue && options.Cluster.Value)
            {
                parameters.ShardCount = options.MinShards.Value;
            }

            if (options.Zones != null)
            {
                foreach (var s in options.Zones)
                {
                    parameters.Zones.Add(s.ToString());
                }
            }

            if (options.RedisVersion != null)
            {
                parameters.RedisVersion = options.RedisVersion;
            }
            // If RedisVersion is not specified, default will be used

            if (forceCreateReplicas || (options.ReplicasPerPrimary.HasValue && skuobj.Name.ToString() == "Premium"))
            {
                parameters.ReplicasPerMaster = options.ReplicasPerPrimary;
                parameters.ReplicasPerPrimary = options.ReplicasPerPrimary;
            }

            return parameters;
        }

        private static RedisEnterpriseClusterData GenerateClusterParams(EnterpriseCacheOptions options)
        {
            var location = new AzureLocation(options.RegionName);

            var clusterParams = new RedisEnterpriseClusterData(options.RegionName, new RedisEnterpriseSku(options.SkuName));

            if (options?.Zones?.Length > 0)
            {
                foreach (var s in options.Zones)
                {
                    clusterParams.Zones.Add(s);
                }

            }
            return clusterParams;

        }

        private static RedisEnterpriseDatabaseData GenerateDatabaseParams(ArmClient arm, EnterpriseCacheOptions options)
        {
            var databaseParams = new RedisEnterpriseDatabaseData();
            //var databaseParams = new RedisEnterpriseDatabaseResource();

            if (options?.Modules?.Length > 0)
            {
                var modules = new List<RedisEnterpriseModule>();
                foreach (var module in options.Modules)
                {
                    databaseParams.Modules.Add(new RedisEnterpriseModule("foo")
                    {
                        Name = module.ToString(),
                    });
                }

            }
            else if (options?.Persistence == EnterpriseCacheOptions.PersistenceType.RDB)
            {
                databaseParams.Persistence = new RedisPersistenceSettings()
                {
                    IsRdbEnabled = true,
                    RdbFrequency = "1h"
                };
            }
            else if (options?.Persistence == EnterpriseCacheOptions.PersistenceType.AOF)
            {
                databaseParams.Persistence = new RedisPersistenceSettings()
                {
                    IsAofEnabled = true,
                    AofFrequency = "1s"
                };
            }
            return databaseParams;
        }

       
    }
}
