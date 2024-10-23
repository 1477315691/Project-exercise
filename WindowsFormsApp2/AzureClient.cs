using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Redis;
using Azure.ResourceManager.RedisEnterprise;
using Azure.ResourceManager.Resources;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    /// <summary>
    /// Client to communicate with Azure
    /// </summary>
    public class AzureClient
    {
        public ArmClient ArmClient { get; set; }

        public ResourceGroupResource ResourceGroupResource { get; set; }

        public string ResourceGroupName { get; set; }

        public AzureLocation Location { get; set; }

        public SubscriptionResource Subscription { get; set; }

        public ResourceGroupCollection ResourceCollection { get; set; }

        public RedisCollection RedisCollection { get; set; }

        public RedisEnterpriseClusterCollection RedisEnterpriseCollection { get; set; }

        private AzureClient() { }

        public static ArrayList CacheGroup = new ArrayList();
        
        public static AzureClient cli = null;
        // TODO: move to a mangager class
        public static async Task<AzureClient> InitializeAzureClientAsync(AzureLocation location,
            string resourceGroupName)
        {
            var client = new AzureClient
            {
                Location = location,
                ResourceGroupName = resourceGroupName
            };

            ArmClient? armClient = null;
            string subscriptionId = "1e57c478-0901-4c02-8d35-49db234b78d2";
            // For instances where application id is not loaded, like, executing on local dev box in outlook based tenant
            // for dogfood/stage environments, use powershell context. You can login using powershell 
            // 'Login-AzAccount -Environment Dogfood'

            armClient = new ArmClient(new DefaultAzureCredential(),subscriptionId);

            client.ArmClient = armClient;

            client.Subscription = armClient.GetSubscriptionResource(new ResourceIdentifier("/subscriptions/" + subscriptionId));
            client.ResourceCollection = client.Subscription.GetResourceGroups();

            client.ResourceGroupResource = client.Subscription.GetResourceGroups().Get(resourceGroupName);
            client.RedisCollection = client.ResourceGroupResource.GetAllRedis();
            client.RedisEnterpriseCollection = client.ResourceGroupResource.GetRedisEnterpriseClusters();
            cli = client;
            return client;
        }
        public static async Task<AzureClient> InitializeAzureClientAsync_2(AzureLocation location,
            string resourceGroupName)
        {
            var client = new AzureClient
            {
                Location = location,
                ResourceGroupName = resourceGroupName
            };

            ArmClient? armClient = null;
            string subscriptionId = "fc2f20f5-602a-4ebd-97e6-4fae3f1f6424";
            // For instances where application id is not loaded, like, executing on local dev box in outlook based tenant
            // for dogfood/stage environments, use powershell context. You can login using powershell 
            // 'Login-AzAccount -Environment Dogfood'

            armClient = new ArmClient(new DefaultAzureCredential(), subscriptionId);

            client.ArmClient = armClient;

            client.Subscription = armClient.GetSubscriptionResource(new ResourceIdentifier("/subscriptions/" + subscriptionId));
            client.ResourceCollection = client.Subscription.GetResourceGroups();

            client.ResourceGroupResource = client.Subscription.GetResourceGroups().Get(resourceGroupName);
            client.RedisCollection = client.ResourceGroupResource.GetAllRedis();
            client.RedisEnterpriseCollection = client.ResourceGroupResource.GetRedisEnterpriseClusters();
            cli = client;
            return client;
        }


        public static async Task<RedisResource> GetCacheByName(string groupName, string cacheName)

        {
            var cache = (await cli.Subscription.GetResourceGroupAsync(groupName)).Value.GetRedisAsync(cacheName).Result.Value;

            return cache;


        }

        public static async Task<ArrayList> GetCacheByGroup(string Name)

        {

            CacheGroup.Clear();
            foreach (var item in (await cli.Subscription.GetResourceGroupAsync(Name)).Value.GetAllRedis())
            {
                Console.WriteLine(item.Data.Name);
                CacheGroup.Add(item.Data.Name);
            }


            return CacheGroup;


        }

        public static async Task<ArrayList> GetGroupLis() 
        {
            ArrayList GroupLis = new ArrayList();
            foreach (var group in cli.Subscription.GetResourceGroups())
            {

                GroupLis.Add(group.Data.Name);
                //foreach (var item in client.Azure.RedisCaches.ListByResourceGroup(group.Name))
                //{
                //Console.WriteLine(item.Name);
                //client.Azure.RedisCaches.DeleteByResourceGroup(group.Name, item.Name);
                //}
            };
            return GroupLis;
        }

    }
}
