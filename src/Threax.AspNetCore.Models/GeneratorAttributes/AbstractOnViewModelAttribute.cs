using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AbstractOnViewModelAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-abstract-viewmodel";

        public AbstractOnViewModelAttribute(bool abs = true) : base(Name, abs)
        {
        }
    }

    public static class AbstractOnViewModelAttributeJsonSchemaExtensions
    {
        public static bool IsAbstractOnViewModel(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(AbstractOnViewModelAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}
