using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Do not generate an entity class for this model or do not include the property on the entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NoEntityAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-entity";

        public NoEntityAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoEntityAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an entity.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateEntity(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoEntityAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
