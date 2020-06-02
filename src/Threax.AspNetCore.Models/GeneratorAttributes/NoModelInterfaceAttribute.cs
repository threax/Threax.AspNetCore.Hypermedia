using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Do not generate an entity class for this model or do not include the property on the entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NoModelInterfaceAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-model-interface";

        public NoModelInterfaceAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoModelInterfaceAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an entity.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateModelInterface(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoModelInterfaceAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
