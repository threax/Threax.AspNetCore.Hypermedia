using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute marks a property abstract on the generated side of the query model. You must implement the
    /// property yourself on the user side of the model, but this allows you to customize behavior or add
    /// additional attributes not supported by the model generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
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
