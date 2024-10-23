using Azure.ResourceManager.RedisEnterprise;
using Azure.Core;
using Azure.ResourceManager.RedisEnterprise.Models;
using System;
using System.Linq;

namespace WindowsFormsApp2
{
    public class EnterpriseCacheOptions
    {
        // TODO: add more options here
        public enum Module
        {
            RedisBloom,
            RediSearch,
            RedisTimeSeries,
            RedisJSON
        }

        public enum PersistenceType
        {
            RDB,
            AOF
        }
        public ResourceIdentifier SubnetId { get; set; }

        public string ClusteringPolicy { get; set; } = "OSSCluster";

        public string ClientProtocol { get; set; } = "Encrypted";

        public RedisEnterpriseSkuName SkuName { get; set; }

        public string RegionName { get; set; }

        public int Capacity { get; set; }

        public Module[]? Modules { get; set; }

        public string[] Zones { get; set; }

        public PersistenceType Persistence { get; set; }

        public string CachePrefix { get; set; }

        public bool Matches(RedisEnterpriseClusterResource cache, AzureLocation defaultRegion)
        {
            if (!cache.Data.Sku.Name.Equals(SkuName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the selected options are supported by the specified region.
        /// 
        /// This is a best effort implementation that will need to be updated over time.
        /// 
        /// If returns false, unsupportedReason will contain a string explaining which feature is not supported.
        /// </summary>
        /// <returns></returns>
        public bool IsSupportedByRegion(out string unsupportedReason)
        {
            if (RegionName.Equals("eastus2euap", StringComparison.InvariantCultureIgnoreCase))
            {
                unsupportedReason = "East US 2 EUAP does not support Enterprise skus";
                return false;
            }

            if (RegionName.Equals("centraluseuap", StringComparison.InvariantCultureIgnoreCase))
            {
                if (Zones?.Length > 0)
                {
                    unsupportedReason = "Central US EUAP does not support zones";
                    return false;
                }

                var flashSkus = new RedisEnterpriseSkuName[] { RedisEnterpriseSkuName.EnterpriseFlashF300, RedisEnterpriseSkuName.EnterpriseFlashF700, RedisEnterpriseSkuName.EnterpriseFlashF1500 };
                if (flashSkus.Contains(SkuName))
                {
                    unsupportedReason = "Central US EUAP does not support Enterprise Flash skus";
                    return false;
                }
            }

            unsupportedReason = null;
            return true;
        }
    }
}
