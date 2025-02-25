using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Community.AspNetCore.DataProtection.DistributedCache
{
    internal sealed class DistributedCacheXmlRepository : IXmlRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly string _cacheKey;

        public DistributedCacheXmlRepository(IDistributedCache distributedCache, string cacheKey = "DataProtection-Keys")
        {
            _distributedCache = distributedCache;
            _cacheKey = cacheKey;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var data = _distributedCache.GetString(_cacheKey);

            if (!string.IsNullOrEmpty(data))
            {
                return XDocument.Parse(data)
                    .Root!
                    .Elements()
                    .ToList()
                    .AsReadOnly();
            }

            return Array.Empty<XElement>();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var data = _distributedCache.GetString(_cacheKey);

            var doc = string.IsNullOrEmpty(data) ? 
                new XDocument(new XElement("keys")) : XDocument.Parse(data);

            doc.Root!.Add(element);

            _distributedCache.SetString(_cacheKey, doc.ToString());
        }
    }
}
