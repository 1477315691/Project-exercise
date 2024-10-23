using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using System.Threading.Tasks;
using WindowsFormsApp2;

public class CreateBVTCacheTest
{
    private Task<AzureClient> azureClient;
    private RedisEnterpriseClusterCollection redisEnterpriseCC;
    public PrivateEndpointResource PrivateEndpoint;
    public ArmClient ArmClient { get; set; }

    public CreateBVTCacheTest()
    {
        azureClient = AzureClient.InitializeAzureClientAsync(
                new AzureLocation("centraluseuap"), //new AzureLocation("eastus2euap")
                "PortalBVT");
        redisEnterpriseCC = azureClient.Result.RedisEnterpriseCollection;

        //create StorageAccount
        string storageAccountName = "bvtatwus2";

        StorageAccountCollection accountCollection = azureClient.Result.ResourceGroupResource.GetStorageAccounts();
        StorageAccountCreateOrUpdateContent parameters = new StorageAccountCreateOrUpdateContent(
                new StorageSku(StorageSkuName.StandardLrs),
                "StorageV2",
                "West US 2");
        // Get storage account connection string
        StorageAccountResource accountResource = accountCollection.CreateOrUpdateAsync(WaitUntil.Completed,
                storageAccountName,
                parameters).Result.Value;

        //create VirtualNetwork
        var virtualNetworkCC = azureClient.Result.ResourceGroupResource.GetVirtualNetworks();
        var virtualNetworkParameter = new VirtualNetworkData()
        {
            Location = "West US 2",

        };
        virtualNetworkParameter.AddressPrefixes.Add("10.0.2.0/24");
        SubnetData subData = new SubnetData();
        subData.Name = "subnetwus2";
        subData.AddressPrefix = "10.0.2.0/24";

        virtualNetworkParameter.Subnets.Add(subData);

        VirtualNetworkResource virtualNetwork = null;

        virtualNetwork = virtualNetworkCC.CreateOrUpdateAsync(WaitUntil.Completed, "vnetwus2", virtualNetworkParameter).Result.Value;

    }
    public async Task Case1()
    {
        //case id: 25113987
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };

       
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-FlushBlade-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case2()
    {
        //case id: 17862550
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-DataAccess-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case3()
    {
        //case id: 14644095
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-OverviewBlade-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case4()
    {
        //case id: 14630744
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-AccessKeys-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case5()
    {
        //case id: 24949331
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-Authentication-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case6()
    {
        //case id: 14630751
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-AdvancedSettings-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case7()
    {
        //case id: 14630803
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-Reboot-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case8()
    {
        //case id: 14630756
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            Cluster = true,
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-ClusterSize-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case9()
    {
        //case id: 14630757
        var options = new RedisCacheOptions()
        {
            SkuName = "Standard",
            RegionName = "West US 2",
            NonSSL = true,
        };

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };
        Task<RedisResource> cache = RedisClient.CreateRedisResource("BVTtest-DataPersistence-WUS2-standard", options,
        azureClient.Result.RedisCollection, false);

        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("BVTtest-DataPersistence-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case10()
    {
        //case id: 14630760
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-ManagedIdentity-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case11()
    {
        //case id: 14630767
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-ScheduleUpdates-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case12()
    {
        //case id: 14630768
        var options1 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "Central US EUAP",
            NonSSL = true,
        };
        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-Geo-replication-cuse", options1,
        azureClient.Result.RedisCollection, false);

        var options2 = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "East US",
            NonSSL = true,
        };
        Task<RedisResource> cache2 = RedisClient.CreateRedisResource("BVTtest-Geo-replication-eus", options2,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case13()
    {
        //case id: 14630773
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-VirtualNetwork-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case14()
    {
        //case id: 14630774
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-PrivateEndpoint-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case15()
    {
        //case id: 14630782
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-FirewallBlade-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
    public async Task Case16()
    {
        //case id: 14630783
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-PropertiesBlade-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case17()
    {
        //case id: 14630798
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-Import-Export-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case18()
    {
        //case id: 14630809
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-PortalOwned-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }

    public async Task Case19()
    {
        //case id: 14630748
        var options = new RedisCacheOptions()
        {
            SkuName = "Premium",
            RegionName = "West US 2",
            NonSSL = true,
        };


        Task<RedisResource> cache1 = RedisClient.CreateRedisResource("BVTtest-Localization-WUS2-premium", options,
        azureClient.Result.RedisCollection, false);
    }
}