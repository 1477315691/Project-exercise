using System;

namespace WindowsFormsApp2
{
    /// <summary>
    /// Represents a bit field which we can perform bitwise operations on. Enumeration constants are defined in powers
    /// of 2. Values between constants represent the logical OR of the different flags. <see href="https://learn.microsoft.com/en-us/dotnet/api/system.flagsattribute"/>
    /// </summary>
    [Flags]
    public enum CacheFeatures
    {
        None = 0,
        Cluster = 1 << 0, //TODO: Use this constant 
        MRPP = 1 << 1,
        Zones = 1 << 2, //Defaults to 2 zones for now
        CloudService = 1 << 3,
        ManagedIdentitySystem = 1 << 4,
        ManagedIdentityUser = 1 << 5,
        ManagedIdentitySystemAndUser = CacheFeatures.ManagedIdentitySystem | CacheFeatures.ManagedIdentityUser,
        AAD = 1 << 6
    }

    public static class CacheFeaturesHelper
    {
        public static readonly string CloudServiceCacheNamePrefix = "FTCS";
        public static bool IsCloudService(CacheFeatures cacheFeatures) => cacheFeatures.HasFlag(CacheFeatures.CloudService);

    }
}
