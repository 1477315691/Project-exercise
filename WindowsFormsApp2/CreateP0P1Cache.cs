using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.Redis.Models;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp2;

public class CreateP0P1CacheTest
{
    private Task<AzureClient> azureClient;
    private RedisEnterpriseClusterCollection redisEnterpriseCC;
    public PrivateEndpointResource PrivateEndpoint;
    public ArmClient ArmClient { get; set; }

    public CreateP0P1CacheTest()
    {
        azureClient = AzureClient.InitializeAzureClientAsync(
                new AzureLocation("centraluseuap"), //new AzureLocation("eastus2euap")
                "PortalBVT");
        redisEnterpriseCC = azureClient.Result.RedisEnterpriseCollection;

        //create StorageAccount
        string storageAccountName = "p0p1atcuse";

        StorageAccountCollection accountCollection = azureClient.Result.ResourceGroupResource.GetStorageAccounts();
        StorageAccountCreateOrUpdateContent parameters = new StorageAccountCreateOrUpdateContent(
                new StorageSku(StorageSkuName.StandardLrs),
                "StorageV2",
                "Central US EUAP");
        // Get storage account connection string
        StorageAccountResource accountResource = accountCollection.CreateOrUpdateAsync(WaitUntil.Completed,
                storageAccountName,
                parameters).Result.Value;

        string storageAccountName2 = "p0p1ateus2e";

        StorageAccountCollection accountCollection2 = azureClient.Result.ResourceGroupResource.GetStorageAccounts();
        StorageAccountCreateOrUpdateContent parameters2 = new StorageAccountCreateOrUpdateContent(
                new StorageSku(StorageSkuName.StandardLrs),
                "StorageV2",
                "East US 2 EUAP");
        // Get storage account connection string
        StorageAccountResource accountResource2 = accountCollection2.CreateOrUpdateAsync(WaitUntil.Completed,
                storageAccountName2,
                parameters2).Result.Value;

        //create VirtualNetwork
        var virtualNetworkCC = azureClient.Result.ResourceGroupResource.GetVirtualNetworks();
        var virtualNetworkParameter = new VirtualNetworkData()
        {
            Location = "Central US EUAP",

        };
        virtualNetworkParameter.AddressPrefixes.Add("10.0.2.0/24");
        SubnetData subData = new SubnetData();
        subData.Name = "subnetcuse";
        subData.AddressPrefix = "10.0.2.0/24";

        virtualNetworkParameter.Subnets.Add(subData);

        VirtualNetworkResource virtualNetwork = null;

        virtualNetwork = virtualNetworkCC.CreateOrUpdateAsync(WaitUntil.Completed, "vnetcuse", virtualNetworkParameter).Result.Value;

        var virtualNetworkParameter2 = new VirtualNetworkData()
        {
            Location = "East US 2 EUAP",

        };
        virtualNetworkParameter2.AddressPrefixes.Add("10.0.2.0/24");
        SubnetData subData2 = new SubnetData();
        subData2.Name = "subneteus2e";
        subData2.AddressPrefix = "10.0.2.0/24";

        virtualNetworkParameter2.Subnets.Add(subData);
        VirtualNetworkResource virtualNetwork2 = null;

        virtualNetwork2 = virtualNetworkCC.CreateOrUpdateAsync(WaitUntil.Completed, "vneteus2e", virtualNetworkParameter2).Result.Value;
    }
    
    public async Task Case1()
    {
        //case id: 15318672
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Standard",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };

        var options2 = new EnterpriseCacheOptions
        {
            SkuName = "Enterprise_E10",
            Capacity = 2,
            RegionName = "Central US EUAP",
        };

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Standard",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15318672-cuse-standard", options1,
        azureClient.Result.RedisCollection, false);


        Task<RedisEnterpriseClusterResource> cache2 = RedisClient.CreateRedisEnterpriseCacheAsync(azureClient.Result, "Manualtest-15318672-cuse-enterprise",
                options2,
                redisEnterpriseCC);

        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-15318672-eus2e-standard", options3,
        azureClient.Result.RedisCollection, false);

        //need to test manually about eus2e enterprise cache

    }

    public async Task Case2()
    {
        //case id: 15318659
        var options1 = new EnterpriseCacheOptions
        {
            SkuName = "Enterprise_E10",
            Capacity = 2,
            RegionName = "Central US EUAP",
        };
        Task<RedisEnterpriseClusterResource> cache1 = RedisClient.CreateRedisEnterpriseCacheAsync(azureClient.Result, "Manualtest-15318659-cuse-enterprise",
                options1,
                redisEnterpriseCC);

        //need to test manually about eus2e enterprise cache
    }

    public async Task Case3()
    {
        //case id: 15318673
        //need to test manually about cuse p1 zone cache
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Zones = new List<string>{
                "1","2"}
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15318673-eus2e-creathendelete", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Zones = new List<string>{
                "1","2"},
            ReplicasPerPrimary = 2,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15318673-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case4()
    {
        //case id: 15318674
        var virtualNetwork = azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vnetcuse");
        var virtualNetwork2 = azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vneteus2e");

        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork.Result.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "Central US EUAP",
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15318674-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork2.Result.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "East US 2 EUAP",
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15318674-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case5()
    {
        //case id: 15318675
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15318675-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15318675-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case6()
    {
        //case id: 15319070
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
            Cluster = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15319070-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Cluster = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15319070-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case7()
    {
        //case id: 15319116
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
            ReplicasPerPrimary = 2,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15319116-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
            ReplicasPerPrimary = 2,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15319116-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case8()
    {
        //case id: 15320703
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15320703-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15320703-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
        };
        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-15320703-eus", options3,
        azureClient.Result.RedisCollection, false);

        var options4 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Southeast Asia",
            NonSSL = true,
        };
        Task<RedisResource> cache4 = RedisClient.CreateRedisResource("Manualtest-15320703-sea", options4,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case9()
    {
        //case id: 15379484
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15379484-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15379484-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case10()
    {
        //case id: 15379484
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15379484-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var virtualNetworkCC = azureClient.Result.ResourceGroupResource.GetVirtualNetworks();
        //  Update Subnet
        SubnetData modSub = virtualNetworkCC.GetAsync("vnetcuse").Result.Value.Data.Subnets.First();
        //4. Set up the private endpoint for cache
        PrivateEndpointData privateEndpointForCache = new PrivateEndpointData();

        privateEndpointForCache.Subnet = modSub;

        string privateEndpointName = "PrivateEndpointcuse";

        privateEndpointForCache.Location = "Central US EUAP";
        var networkPLSC = new NetworkPrivateLinkServiceConnection()
        {
            Name = privateEndpointName,
            RequestMessage = "Please approve my connection",
            PrivateLinkServiceId = cache1.Result.Data.Id,

        };
        networkPLSC.GroupIds.Add("redisCache");
        privateEndpointForCache.PrivateLinkServiceConnections.Add(networkPLSC);

        PrivateEndpointCollection pepc = ArmClient.GetResourceGroupResource(new ResourceIdentifier("/subscriptions/" + "1e57c478-0901-4c02-8d35-49db234b78d2" + "/resourceGroups/" + "PortalBVT")).GetPrivateEndpoints();

        PrivateEndpoint = pepc.CreateOrUpdateAsync(WaitUntil.Completed, privateEndpointName, privateEndpointForCache).Result.Value;
        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15379484-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        //  Update Subnet
        SubnetData modSub2 = virtualNetworkCC.GetAsync("vneteus2e").Result.Value.Data.Subnets.First();
        //4. Set up the private endpoint for cache
        PrivateEndpointData privateEndpointForCache2 = new PrivateEndpointData();

        privateEndpointForCache.Subnet = modSub;

        string privateEndpointName2 = "PrivateEndpointeus2e";

        privateEndpointForCache.Location = "East US 2 EUAP";
        var networkPLSC2 = new NetworkPrivateLinkServiceConnection()
        {
            Name = privateEndpointName2,
            RequestMessage = "Please approve my connection",
            PrivateLinkServiceId = cache2.Result.Data.Id,

        };
        networkPLSC2.GroupIds.Add("redisCache");
        privateEndpointForCache2.PrivateLinkServiceConnections.Add(networkPLSC2);

        PrivateEndpointCollection pepc2 = ArmClient.GetResourceGroupResource(new ResourceIdentifier("/subscriptions/" + "1e57c478-0901-4c02-8d35-49db234b78d2" + "/resourceGroups/" + "PortalBVT")).GetPrivateEndpoints();
    }
    public async Task Case11()
    {
        //case id: 15379676
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15379676-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15379676-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case12()
    {
        //case id: 15379764
        StorageAccountResource accountResource = azureClient.Result.ResourceGroupResource.GetStorageAccount("p0p1atcuse");
        StorageAccountResource accountResource2 = azureClient.Result.ResourceGroupResource.GetStorageAccount("p0p1ateus2e");

        var primaryKey = accountResource.GetKeys().First().Value;
        var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountResource.Data.Name};AccountKey={primaryKey};EndpointSuffix=core.windows.net";

        // Enable AOF persistence
        var configuration = new RedisCommonConfiguration();
        configuration.IsAofBackupEnabled = true;

        configuration.AofStorageConnectionString1 = connectionString;
        configuration.AofStorageConnectionString0 = null;

        var primaryKey2 = accountResource.GetKeys().First().Value;
        var connectionString2 = $"DefaultEndpointsProtocol=https;AccountName={accountResource2.Data.Name};AccountKey={primaryKey2};EndpointSuffix=core.windows.net";
        var configuration2 = new RedisCommonConfiguration();
        configuration2.IsAofBackupEnabled = true;

        configuration2.AofStorageConnectionString1 = connectionString2;
        configuration2.AofStorageConnectionString0 = null;

        var virtualNetwork = await azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vnetcuse");
        var virtualNetwork2 = await azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vneteus2e");


        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "Central US EUAP",
            Cluster = true,
            redisCommonConfiguration = configuration,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15318764-cuse1", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork2.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "East US 2 EUAP",
            redisCommonConfiguration = configuration2,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15318764-eus2e1", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case13()
    {
        //case id: 15379874
        var virtualNetwork = azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vnetcuse");
        var virtualNetwork2 = azureClient.Result.ResourceGroupResource.GetVirtualNetworkAsync("vneteus2e");

        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork.Result.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "Central US EUAP",
            ReplicasPerPrimary = 3,
            Cluster = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15379874-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            SubnetId = virtualNetwork2.Result.Value.Data.Subnets[0].Id,
            ForceCreate = true,
            CacheFeatures = CacheFeatures.None,
            RegionName = "East US 2 EUAP",
            ReplicasPerPrimary = 3,
            Cluster = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15379874-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case14()
    {
        //case id: 15380305
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            Cluster = true,
            MinShards = 2,
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-15380305-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-15380305-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
        };
        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-15380305-eus", options3,
        azureClient.Result.RedisCollection, false);

        var options4 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Southeast Asia",
            NonSSL = true,
            Cluster = true,
            MinShards = 2,
        };
        Task<RedisResource> cache4 = RedisClient.CreateRedisResource("Manualtest-15380305-sea", options4,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case15()
    {
        //case id: 16021106
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-16021106-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-16021106-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
        };
        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-16021106-eus", options3,
        azureClient.Result.RedisCollection, false);

        var options4 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Southeast Asia",
            NonSSL = true,
        };
        Task<RedisResource> cache4 = RedisClient.CreateRedisResource("Manualtest-16021106-sea", options4,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case16()
    {
        //case id: 16021140
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-16021140-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-16021140-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
        };
        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-16021140-eus", options3,
        azureClient.Result.RedisCollection, false);

        var options4 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Southeast Asia",
            NonSSL = true,
        };
        Task<RedisResource> cache4 = RedisClient.CreateRedisResource("Manualtest-16021140-sea", options4,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case17()
    {
        //case id: 16021226
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-16021226-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-16021226-eus2e", options2,
        azureClient.Result.RedisCollection, false);

        var options3 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
        };
        Task<RedisResource> cache3 = RedisClient.CreateRedisResource("Manualtest-16021226-eus", options3,
        azureClient.Result.RedisCollection, false);

        var options4 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Southeast Asia",
            NonSSL = true,
        };
        Task<RedisResource> cache4 = RedisClient.CreateRedisResource("Manualtest-16021226-sea", options4,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case18()
    {
        //case id: 20755069
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Manualtest-20755069-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US 2 EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("Manualtest-20755069-eus2e", options2,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case19()
    {
        //case id: 24879297
        var options1 = new EnterpriseCacheOptions
        {
            SkuName = "Enterprise_E10",
            Capacity = 2,
            RegionName = "Central US EUAP",
        };
        Task<RedisEnterpriseClusterResource> cache1 = RedisClient.CreateRedisEnterpriseCacheAsync(azureClient.Result, "Manualtest-24879297-cuse-enterprise",
                options1,
                redisEnterpriseCC);
        //need to test manually about eus2e enterprise cache
    }
    public async Task Case20()
    {
        //need to test manually because need to create resourcegroup
    }
}