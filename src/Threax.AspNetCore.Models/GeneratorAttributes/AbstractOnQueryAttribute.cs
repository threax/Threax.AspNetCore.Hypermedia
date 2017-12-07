using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AbstractOnQueryAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-abstract-query";

        public AbstractOnQueryAttribute(bool abs = true) : base(Name, abs)
        {
        }
    }

    public static class AbstractOnQueryAttributeJsonSchemaExtensions
    {
        public static bool IsAbstractOnQuery(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(AbstractOnQueryAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}
