using System;
using System.Collections.Generic;
using System.Text;
using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This attribute will mark a type as able to have its endpoint docs cached. This can help a lot in
    /// low performance scenerios and won't use much memory. This attribute is safe to use on most types,
    /// however, if you have a dynamic value provider, like one that reads from a database, you should avoid
    /// using this attribute on that type. Value provides that provide static values or enum values are safe
    /// to use with this type. Basically if there is no logic to creating the Json Schema you can use this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheEndpointDocAttribute : JsonSchemaExtensionDataAttribute
    {
        public CacheEndpointDocAttribute()
            : base(CacheEndpointDocSchemaExtensions.Property, true)
        {

        }
    }

    public static class CacheEndpointDocSchemaExtensions
    {
        internal const string Property = "cacheabledocs";

        public static bool IsCacheableDoc(this JsonSchema4 jsonSchema4)
        {
            object cacheable = null;
            jsonSchema4.ExtensionData?.TryGetValue(Property, out cacheable);
            return (cacheable as bool?) == true;
        }
    }
}
