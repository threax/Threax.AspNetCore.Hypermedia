using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Set the name of the generated key property for this model. Otherwise defaults to ModelId.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class KeyNameAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-key-name";

        public KeyNameAttribute(String name) : base(Name, name)
        {
        }
    }

    public static class KeyNameAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the name to use for the key attribute.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetKeyName(this JsonSchema4 schema)
        {
            Object val = null;
            if(schema.ExtensionData?.TryGetValue(KeyNameAttribute.Name, out val) != true)
            {
                return schema.Title + "Id";
            }
            return val.ToString();
        }
    }
}
