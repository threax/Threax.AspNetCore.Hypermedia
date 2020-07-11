using System;
using System.Threading.Tasks;
using Threax.NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocCache
    {
        Task<EndpointDocCacheResult> GetCachedRequest(string groupName, string method, string relativePath, Type type, Func<Type, Task<JsonSchema4>> missCallback);
        Task<EndpointDocCacheResult> GetCachedResponse(string groupName, string method, string relativePath, Type type, Func<Type, Task<JsonSchema4>> missCallback);
    }
}