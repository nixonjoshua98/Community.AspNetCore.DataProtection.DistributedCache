using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Community.AspNetCore.DataProtection.DistributedCache
{
    public static class DistributedCacheDataProtectionExtensions
    {
        public static IDataProtectionBuilder PersistKeysToDistributedCache(this IDataProtectionBuilder builder, string key = "DataProtectionKeys")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddSingleton<IXmlRepository, DistributedCacheXmlRepository>();

            builder.Services.AddSingleton(new DistributedCacheOptions(key));

            return builder;
        }

        public static IDataProtectionBuilder PersistKeysToDistributedCache(this IDataProtectionBuilder builder, IDistributedCache distributedCache, string key = "DataProtectionKeys")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new DistributedCacheXmlRepository(distributedCache, key);
            });

            return builder;
        }
    }
}
