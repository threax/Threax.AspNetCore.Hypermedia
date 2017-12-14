using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute marks a property abstract on the generated side of the view model. You must implement the
    /// property yourself on the user side of the model, but this allows you to customize behavior or add
    /// additional attributes not supported by the model generator.
    /// </summary>
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
