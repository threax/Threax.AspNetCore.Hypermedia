using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Threax.NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class EndpointDocCache : IEndpointDocCache
    {
        private ConcurrentDictionary<String, JsonSchema4> requestCache = new ConcurrentDictionary<String, JsonSchema4>();
        private ConcurrentDictionary<String, JsonSchema4> responseCache = new ConcurrentDictionary<String, JsonSchema4>();

        public Task<EndpointDocCacheResult> GetCachedRequest(string groupName, string method, string relativePath, Type type, Func<Type, Task<JsonSchema4>> missCallback)
        {
            return GetCached(requestCache, groupName, method, relativePath, type, missCallback);
        }

        public Task<EndpointDocCacheResult> GetCachedResponse(string groupName, string method, string relativePath, Type type, Func<Type, Task<JsonSchema4>> missCallback)
        {
            return GetCached(responseCache, groupName, method, relativePath, type, missCallback);
        }

        private static async Task<EndpointDocCacheResult> GetCached(ConcurrentDictionary<string, JsonSchema4> cache, string groupName, string method, string relativePath, Type type, Func<Type, Task<JsonSchema4>> missCallback)
        {
            var key = $"{groupName}|{method}|{relativePath}";
            var result = new EndpointDocCacheResult();
            result.FromCache = cache.TryGetValue(key, out var schema);
            if (!result.FromCache)
            {
                //This can potentially race if not defined.
                //However, there is no real downside to that, this is still nice and thread safe.
                schema = await missCallback.Invoke(type);
                if (schema == null || schema.IsCacheableDoc())
                {
                    //If the schema is null or cacheable, add it to the cache, null values will count as coming from the cache
                    schema = cache.GetOrAdd(key, schema);
                    result.FromCache = true; //This is now technically from the cache too
                }
            }
            result.Schema = schema;

            return result;
        }
    }

    public class EndpointDocCacheResult
    {
        public JsonSchema4 Schema { get; set; }

        public bool FromCache { get; set; }
    }
}
