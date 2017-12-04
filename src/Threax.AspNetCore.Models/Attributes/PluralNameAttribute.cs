using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-plural-title";

        public PluralNameAttribute(String value) : base(Name, value)
        {
        }
    }

    public static class PluralNameAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the plural name of a schema. This will always return something either the value of the
        /// x-plural-title extension data or the schema.Title with an s appended.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetPluralName(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(PluralNameAttribute.Name, out val);
            var plural = val?.ToString();
            if(plural == null)
            {
                plural = schema.Title + "s";
            }
            return plural;
        }
    }
}