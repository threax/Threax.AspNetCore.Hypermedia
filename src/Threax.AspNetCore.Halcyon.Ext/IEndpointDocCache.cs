using System;
using System.Threading.Tasks;
using Threax.NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocCache
    {
        Task<JsonSchema4> GetCachedSchema(Type type, Func<Type, Task<JsonSchema4>> missCallback);
    }
}