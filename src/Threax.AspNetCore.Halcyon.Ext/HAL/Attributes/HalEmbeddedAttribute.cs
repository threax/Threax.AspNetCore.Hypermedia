using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;
using System;

namespace Halcyon.HAL.Attributes {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HalEmbeddedAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "is-hal-embedded";

        public HalEmbeddedAttribute()
            :base(Name, true)
        {
        }
    }

    public static class AbstractOnQueryAttributeJsonSchemaExtensions
    {
        public static bool IsHalEmbedded(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(HalEmbeddedAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}