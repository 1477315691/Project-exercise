using Azure.ResourceManager.Redis;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class CreateCache 
    {
        //如果portal有更新需要其他cache请自己修改代码
        //Cache Creation需要手动创建
        public async Task<RedisResource> CreatePortalCacheAAD() 
        {
            //AAD
            RedisCacheOptions options = new RedisCacheOptions()
            {
                SkuName = "Premium",
                
            };
            var cache = await RedisClient.CreateRedisResource("PortalBVTTestAAD", options, AzureClient.cli.RedisCollection);
            return cache;
        }

        public async Task<RedisResource> CreatePortalAKBT()
        {
            //AAD
            RedisCacheOptions options = new RedisCacheOptions()
            {
                SkuName = "Premium",

            };
            var cache = await RedisClient.CreateRedisResource("PortalBVTTestAKBT", options, AzureClient.cli.RedisCollection);
            return cache;
        }

        public async Task<RedisResource> CreatePortalSBT()
        {
            //AAD
            RedisCacheOptions options = new RedisCacheOptions()
            {
                SkuName = "Standard",

            };
            var cache = await RedisClient.CreateRedisResource("PortalBVTTestScaleBladeTest", options, AzureClient.cli.RedisCollection);
            return cache;
        }

        public async Task<RedisResource> CreatePortalRebootBlade()
        {
            //AAD
            RedisCacheOptions options = new RedisCacheOptions()
            {
                SkuName = "Premium",
                Cluster = true,
                MinShards = 3
            };
            var cache = await RedisClient.CreateRedisResource("PortalBVTTestRebootBladeTest", options, AzureClient.cli.RedisCollection);
            return cache;
        }
    }
}
