using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute marks a property abstract on the generated side of the input model. You must implement the
    /// property yourself on the user side of the model, but this allows you to customize behavior or add
    /// additional attributes not supported by the model generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
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
