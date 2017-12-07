using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AbstractOnInputModelAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-abstract-inputmodel";

        public AbstractOnInputModelAttribute(bool abs = true) : base(Name, abs)
        {
        }
    }

    public static class AbstractOnInputModelAttributeJsonSchemaExtensions
    {
        public static bool IsAbstractOnInputModel(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(AbstractOnInputModelAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}
