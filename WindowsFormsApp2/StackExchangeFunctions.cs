using Azure.ResourceManager.Redis;
using StackExchange.Redis;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StackExchangeFunctions
{
    public static class StackExchangeFunctions
    {

        public static string GetPersistenceContainerName(this RedisResource cache)
            => $"{cache.Data.Name}-redis-persistence".Substring(0, 63).Trim('-').ToLower();

        public static async Task FlushData(this RedisResource RedisCache)
        {
            var mx = await GetConnectionMultiplexerWithCredentialAsync(RedisCache);
            if (RedisCache.Data.ShardCount == 0 || RedisCache.Data.ShardCount == null)
            {
                // single primary
                mx.GetServer(RedisCache.Data.HostName, 6380).FlushAllDatabases();
            }
            else
            {
                // Flush all primaries in cluster
                var clusterConfig = mx.GetServer(mx.GetEndPoints().First()).ClusterConfiguration;
                var primaryNodes = clusterConfig.Nodes.Where(n => !n.IsReplica);   
                foreach (var node in primaryNodes)
                {
                    mx.GetServer(node.EndPoint).FlushAllDatabases();
                }
            }
            mx.Close();
        }

        public static async Task<bool> AddData(this RedisResource RedisCache, string key, string val)
        {
            ConnectionMultiplexer mx = await GetConnectionMultiplexerWithCredentialAsync(RedisCache);
            IDatabase db = mx.GetDatabase(0);
            var reply = db.StringSet(key, val);
            mx.Close();
            return reply;
        }

        public static async Task RemoveData(this RedisResource RedisCache, string key)
        {
            ConnectionMultiplexer mx = await GetConnectionMultiplexerWithCredentialAsync(RedisCache);
            IDatabase db = mx.GetDatabase(0);
            db.KeyDelete(key);
            mx.Close();
        }

        public static async Task<long> AddDataBulk(this RedisResource RedisCache, long numKeysPerShard, int valueSize, long ttlSeconds = 0)
        {
            ConnectionMultiplexer mx = await GetConnectionMultiplexerWithCredentialAsync(RedisCache);
            IDatabase db = mx.GetDatabase(0);

            // total number of keys set, which is returned at the end of this method
            long totalKeys = 0;

            // generate a random value to use for all keys
            var rand = new Random();
            byte[] value = new byte[valueSize];
            rand.NextBytes(value);

            // in order to quickly populate a cache with data, we execute a small number of Lua scripts on each shard
            var clusterConfig = mx.GetServer(mx.GetEndPoints().First()).ClusterConfiguration;

            // non clustered case
            if (clusterConfig == null)
            {
                AddDataWithLuaScript(mx, numKeysPerShard, value, "", ttlSeconds);
                return numKeysPerShard;
            }

            // clustered case
            foreach (var node in clusterConfig.Nodes.Where(n => !n.IsReplica))
            {
                // numSlots is the number of slots over which to distribute a shard's keys
                int numSlots = Math.Min(100, node.Slots.Sum((range) => range.To - range.From));
                var slots = new List<int>();
                for (int i = 0; i < numSlots; i++)
                {
                    // compute a value that hashes to this node and use it as a hash tag
                    var hashTagAndSlot = GetHashTagForNode(mx, node.Slots, slots, rand);

                    long numKeys = (numKeysPerShard / numSlots) + 1; // +1 so numKeys > 0 when numKeysPerShard < numSlots
                    AddDataWithLuaScript(mx, numKeys, value, hashTagAndSlot.Item1, ttlSeconds);
                    totalKeys += numKeys;
                    slots.Add(hashTagAndSlot.Item2);
                }
            }

            mx.Close();
            return totalKeys;
        }

        public static async Task FillP1CacheToMaxMemoryAsync(this RedisResource cache)
        {
            ConnectionMultiplexer mx = await GetConnectionMultiplexerWithCredentialAsync(cache, syncTimeoutSeconds: 5 * 60);
            //assert not allkeys memory policy, otherwise this fill approach wont work
            IEnumerable<string> initialMaxMemoryPolicies = await cache.GetRedisServerInfoCommandKeyValue("Memory", "maxmemory_policy");
            int valueSize = 1024;
            var rand = new Random();
            byte[] value = new byte[valueSize];
            rand.NextBytes(value);

            AddDataWithLuaScript(mx, 4 * 1000 * 1000, value, "", 0);
        }

        /// <summary>
        /// A function to fill a Standard C1 to full. We rely on redis to inform us when its OOM to know when to stop inserting.
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        //public static async Task FillStandardCache(this RedisResource cache, ITestOutputHelper Log, CacheAccessType accessType = CacheAccessType.ACCESS_KEY, CacheAccessConfig? cacheAccessConfig = null)
        //{
        //    Log.WriteLine("Adding data into non clustered cache");
        //    ConnectionMultiplexer mx = await GetConnectionMultiplexerAsync(cache, syncTimeoutSeconds: 5 * 60, accessType: accessType, cacheAccessConfig: cacheAccessConfig);
        //    var db = mx.GetDatabase(0);

        //    //assert not allkeys memory policy, otherwise this fill approach wont work
        //    IEnumerable<string> initialMaxMemoryPolicies = await cache.GetRedisServerInfoCommandKeyValue("Memory", "maxmemory_policy");
        //    foreach (string maxMemoryPolicy in initialMaxMemoryPolicies)
        //    {
        //        Assert.DoesNotContain("allkeys", maxMemoryPolicy);
        //    }

        //    byte[] value = new byte[1024];
        //    new Random().NextBytes(value);

        //    Stopwatch stopwatch = Stopwatch.StartNew();
        //    int i = 1;
        //    while (true)
        //    {
        //        if (i % 10_000 == 0)
        //        {
        //            // Give redis a chance to tell us it's OOM
        //            try
        //            {
        //                db.StringSet($"key{i}", value);
        //            }
        //            catch (RedisServerException e)
        //            {
        //                if (e.Message.Contains("OOM"))
        //                {
        //                    Log.WriteLine($"OOM at the {i}th iteration");
        //                    break;
        //                }
        //                else
        //                {
        //                    throw e;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Fire away
        //            db.StringSet($"key{i}", value, flags: CommandFlags.FireAndForget);
        //        }
        //        i++;
        //        if (stopwatch.ElapsedMilliseconds > 1000 * 60 * 40)
        //        {
        //            stopwatch.Stop();
        //            throw new TimeoutException("Cache took > 40m to fill");
        //        }
        //    }
        //}


      
        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

 
 
        public static async Task<long> GetKeyCountAsync(this RedisResource RedisCache, bool isGeoSecondaryCache = false, int numOfRetries = 1)
        {
            ConnectionMultiplexer mx = await GetConnectionMultiplexerWithCredentialAsync(RedisCache);
            var clusterConfig = mx.GetServer(mx.GetEndPoints().First()).ClusterConfiguration;

            long numOfKeys = 0;
            for (int i = 0; i < numOfRetries; i++)
            {
                if (clusterConfig == null)
                {
                    return mx.GetServer(mx.GetEndPoints().First()).DatabaseSize(0);
                }
                if (isGeoSecondaryCache)
                {
                    // For geo-secondary caches, all the node's (both primary and replica) redis-server
                    // roles are 'replica' only, hence we won't be able to fetch the node's primary or
                    // replica info using redis-server's role. Hence, using cluster node's isreplica function
                    // to fetch primary nodes of geo-secondary cache to get the total number of keys in the cache.
                    numOfKeys = clusterConfig
                        .Nodes
                        .Where(n => !n.IsReplica)
                        .Select(n => n.EndPoint)
                        .Select(e => mx.GetServer(e))
                        .Sum(s => s.DatabaseSize(0));
                }
                else
                {
                    // For non geo-secondary caches, we can use the redis-server role of nodes to get all the primary
                    // nodes of the cache to get key count. Cluster node's isreplica found to be not returning the real-time
                    // primary/replica info of the nodes when they are under reboot operation, hence server's isreplica function
                    // is used to get key count.
                    numOfKeys = clusterConfig
                        .Nodes
                        .Select(n => n.EndPoint)
                        .Select(e => mx.GetServer(e))
                        .Where(s => !s.IsReplica && s.IsConnected)
                        .Sum(s => s.DatabaseSize(0));
                }
                return numOfKeys;
              
                await Task.Delay(120000);
            }
            return numOfKeys;
        }

        /// <summary>
        /// Gets the total number of evicted keys in the cache.
        /// </summary>
        public static async Task<int> GetEvictedKeyCount(this RedisResource RedisCache)
        {
            IEnumerable<string> evictedKeyCounts = await RedisCache.GetRedisServerInfoCommandKeyValue("Stats", "evicted_keys", true);
            int evictedKeyCount = 0;
            foreach (string keyCount in evictedKeyCounts)
            {
                evictedKeyCount += int.Parse(keyCount);
            }
            return evictedKeyCount;
        }



        /// <summary>
        /// Gets a key from the output returned by the INFO command on the cache.
        /// </summary>
        /// <param name="filterByPrimaries">Only return values from primary nodes</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<IEnumerable<string>> GetRedisServerInfoCommandKeyValue(this RedisResource redisCache,
            string sectionName,
            string keyName, bool filterByPrimaries = false
            )
        {
            List<string> keyresults = new List<string>();
            var mx = await GetConnectionMultiplexerWithCredentialAsync(redisCache);

            EndPoint[] endpoints = mx.GetEndPoints();
            if (filterByPrimaries)
            {
                var clusterConfig = mx.GetServer(endpoints.First()).ClusterConfiguration;
                if (clusterConfig != null)
                {
                    endpoints = clusterConfig.Nodes.Where(n => !n.IsReplica).Select(n => n.EndPoint).ToArray();
                }
                else
                {
                    endpoints = endpoints.Where(ep => !mx.GetServer(ep).IsReplica).ToArray();
                }
            }

            foreach (var endpoint in endpoints)
            {
                var sectionGroup = await mx.GetServer(endpoint).InfoAsync(sectionName);

                var section = sectionGroup.FirstOrDefault(m => m.Key == sectionName);

                if (section == null)
                {
                    throw new ArgumentException($"{sectionName} not found in info results");
                }

                var result = section.FirstOrDefault(s => s.Key == keyName);
                if (result.Equals(default(KeyValuePair<string, string>)))
                {
                    throw new ArgumentException($"{keyName} not found in {sectionName} results");
                }

                keyresults.Add(result.Value);
            }
            return keyresults;
        }

 
        private static Tuple<string, int> GetHashTagForNode(ConnectionMultiplexer mx,
            IList<SlotRange> slotRanges,
            IList<int> excludedSlots,
            Random rand)
        {
            string hashTag = null;
            while (true)
            {
                hashTag = "-{" + rand.Next() + "}-";
                int slot = mx.HashSlot(hashTag);
                if ((excludedSlots == null || !excludedSlots.Contains(slot)) &&
                    slotRanges.Any((range) => range.From <= slot && slot <= range.To))
                {
                    return new Tuple<string, int>(hashTag, slot);
                }
            }
        }

        private static void AddDataWithLuaScript(ConnectionMultiplexer mx, long numKeys, byte[] value, string hashTag, long ttlSeconds = 0, long batchSize = 7_500)
        {
            string script = @"
                        local i = tonumber(@initIndex)
                        while i < tonumber(@numKeys) do
                            if (@ttlSeconds == '0') then
                                redis.call('set', 'mykey'..@hashTag..i, @value)
                            else
                                redis.call('set', 'mykey'..@hashTag..i, @value, 'ex', @ttlSeconds)
                            end
                            i = i + 1
                        end
                    ";

            var prepared = LuaScript.Prepare(script);

            // An attempt to batch these inserts so they're less likely to timeout
            long keysPerInsert = batchSize;
            long i = 0;
            while (keysPerInsert * i < numKeys)
            {
                long insertToValue = Math.Min(numKeys, keysPerInsert * (i + 1)); // On the final iteration we don't want to go to the full multiple of $batchSize
                long startingIndex = keysPerInsert * i;
                mx.GetDatabase(0).ScriptEvaluate(prepared, new
                {
                    initIndex = startingIndex,
                    numKeys = insertToValue,
                    value = value,
                    hashTag = (RedisKey)hashTag,
                    ttlSeconds = ttlSeconds,
                });
                i += 1;
            }
        }

        public static async Task<long> GetCurrentMemoryAsync(this RedisResource redisCache)
        {
            var userMemory = (await GetRedisServerInfoCommandKeyValue(redisCache, "Memory", "used_memory"))
                .FirstOrDefault();

            return long.TryParse(userMemory, out long memory) ? memory : 0;
        }




        private static async Task<ConnectionMultiplexer> GetConnectionMultiplexerWithCredentialAsync(RedisResource RedisCache,
            bool ssl = true,
            int syncTimeoutSeconds = 10
            )
        {
            ConfigurationOptions options = new ConfigurationOptions();
            options.EndPoints.Add($"{RedisCache.Data.HostName}:" + (ssl ? "6380" : "6379"));
            options.Ssl = ssl;
            options.AbortOnConnectFail = false;
            options.AllowAdmin = true;
            options.SyncTimeout = syncTimeoutSeconds * 1000;
            options.ConnectRetry = 3;
            options.ConnectTimeout = Math.Max(syncTimeoutSeconds, 15 * 1000);
            options.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            options.Password = RedisCache.GetKeys().Value.PrimaryKey;
            
            return await ConnectionMultiplexer.ConnectAsync(options);
            
        }
    }
}