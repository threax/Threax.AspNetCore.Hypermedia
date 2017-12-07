using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AbstractOnEntityAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-abstract-entity";

        public AbstractOnEntityAttribute(bool abs = true) : base(Name, abs)
        {
        }
    }

    public static class AbstractOnEntityAttributeAttributeJsonSchemaExtensions
    {
        public static bool IsAbstractOnEntity(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(AbstractOnEntityAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}
