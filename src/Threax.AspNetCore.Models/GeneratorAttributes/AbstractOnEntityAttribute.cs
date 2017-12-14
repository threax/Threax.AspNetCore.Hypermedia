using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute marks a property abstract on the generated side of the entity. You must implement the
    /// property yourself on the user side of the entity, but this allows you to customize behavior or add
    /// additional attributes not supported by the model generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
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
