using Azure.Core;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal class PerformanceCacheCreate
    {
        private Task<AzureClient> azureClient;
        private RedisEnterpriseClusterCollection redisEnterpriseCC;
        public PrivateEndpointResource PrivateEndpoint;
        public ArmClient ArmClient { get; set; }
        string RegionName_1 = "East US 2 EUAP";                    //Central US EUAP
                                                                   //East US 2 EUAP

        static DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("MMdd");
        public async Task Case1()    //创建Pcache
        {
            azureClient = AzureClient.InitializeAzureClientAsync(new AzureLocation("eastus2euap"),
                   //new AzureLocation("eastus2euap")
                   "Cache-FunctionalRun-neverdeletedPatching");
            for (int i = 1; i <= 5; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Premium",
                    SkuCapacity = i,
                    RegionName = RegionName_1,

                    NonSSL = true,
                };

                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Verifyperformance-P" + i + "-EUS2E" + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }
        }
        public async Task Case2()    //创建SCcache
        {
            azureClient = AzureClient.InitializeAzureClientAsync(new AzureLocation("eastus2euap"),
                   //new AzureLocation("eastus2euap")
                   "Cache-FunctionalRun-neverdeletedPatching");
            for (int i = 0; i <= 6; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Standard",
                    SkuCapacity = i,
                    RegionName = RegionName_1,
                    NonSSL = true,
                };
                MessageBox.Show(i.ToString());
                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Verifyperformance-C" + i + "-EUS2E-Standard" + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }

        }
        public async Task Case3()    //创建BCcache
        {
            azureClient = AzureClient.InitializeAzureClientAsync(new AzureLocation("eastus2euap"),
                   //new AzureLocation("eastus2euap")
                   "Cache-FunctionalRun-neverdeletedPatching");
            for (int i = 0; i <= 6; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Basic",
                    SkuCapacity = i,
                    RegionName = RegionName_1,
                    NonSSL = true,
                };

                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("Verifyperformance-C" + i + "-EUS2E-Basic" + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }

        }
    }
}
