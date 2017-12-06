using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NoInputModelAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-input-model";

        public NoInputModelAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoInputModelAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an input model.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateInputModel(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoInputModelAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
