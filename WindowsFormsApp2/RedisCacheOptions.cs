﻿using Azure.ResourceManager.Redis;
using Azure.ResourceManager.Redis.Models;
using Azure.Core;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WindowsFormsApp2
{
    public class RedisCacheOptions
    {
        // TODO: add more options here

        public RedisSkuName SkuName { get; set; }

        public ResourceIdentifier SubnetId { get; set; }

        public RedisCommonConfiguration redisCommonConfiguration { get; set; }

        public bool? Cluster { get; set; }

        public string RedisVersion { get; set; }

        public int? MinShards { get; set; }

        public int? MaxShards { get; set; }

        public bool ForceCreate { get; set; }

        public string RegionName { get; set; }

        public CacheFeatures CacheFeatures { get; set; }

        public bool AllowExistingFirewallRules { get; set; } // default to false, because when we have rules, these tend to break tests...

        public IList<string> Zones { get; set; }

        public int? ReplicasPerPrimary { get; set; }

        public int SkuCapacity { get; set; }

        public bool NonSSL { get; set; }

        public string CachePrefix { get; set; }

    }
}
