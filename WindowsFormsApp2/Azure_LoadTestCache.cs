using Azure.Core;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal class Azure_LoadTestCache
    {
        private Task<AzureClient> azureClient;
        private RedisEnterpriseClusterCollection redisEnterpriseCC;
        public PrivateEndpointResource PrivateEndpoint;
        public ArmClient ArmClient { get; set; }

        static DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("MMdd");
        public async Task Case1()    //创建Pcache
        {

            azureClient = AzureClient.InitializeAzureClientAsync_2(new AzureLocation("centraluseuap"),
                   //new AzureLocation("eastus2euap")
                   "alt-cluster-test");
            for (int i = 1; i <= 5; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Premium",
                    SkuCapacity = i,
                    RegionName = "East US 2 EUAP",
                    NonSSL = true,
                };

                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("alt-eus2e-P" + i + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }
        }
        public async Task Case2()    //创建SCcache
        {

            azureClient = AzureClient.InitializeAzureClientAsync_2(new AzureLocation("centraluseuap"),
                   //new AzureLocation("eastus2euap")
                   "alt-cluster-test");
            for (int i = 0; i <= 6; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Standard",
                    SkuCapacity = i,
                    RegionName = "East US 2 EUAP",
                    NonSSL = true,
                };
                MessageBox.Show(i.ToString());
                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("alt-eus2e-SC" + i + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }

        }
        public async Task Case3()    //创建BCcache
        {

            azureClient = AzureClient.InitializeAzureClientAsync_2(new AzureLocation("centraluseuap"),
                   //new AzureLocation("eastus2euap")
                   "alt-cluster-test");
            for (int i = 0; i <= 6; i++)
            {
                var options1 = new RedisCacheOptions()
                {
                    SkuName = "Basic",
                    SkuCapacity = i,
                    RegionName = "East US 2 EUAP",
                    NonSSL = true,
                };

                Task<RedisResource> cache1 = RedisClient.CreateRedisResource("alt-eus2e-BC" + i + "-" + formattedDate, options1,
                azureClient.Result.RedisCollection, false);
            }
        }
    }
}
