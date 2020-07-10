using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Threax.NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class EndpointDocCache : IEndpointDocCache
    {
        private ConcurrentDictionary<Type, JsonSchema4> cache = new ConcurrentDictionary<Type, JsonSchema4>();

        public async Task<JsonSchema4> GetCachedSchema(Type type, Func<Type, Task<JsonSchema4>> missCallback)
        {
            //Not cacheable, just return the result of the miss callback.
            if (type.GetCustomAttribute<CacheEndpointDocAttribute>() == null)
            {
                return await missCallback(type);
            }

            JsonSchema4 schema;
            if (!cache.ContainsKey(type))
            {
                //This can potentially race if not defined.
                //However, there is no real downside to that, this is still nice and thread safe.
                schema = await missCallback.Invoke(type);
                schema = cache.GetOrAdd(type, schema);

            }
            else
            {
                cache.TryGetValue(type, out schema);
            }

            return schema;
        }
    }
}
