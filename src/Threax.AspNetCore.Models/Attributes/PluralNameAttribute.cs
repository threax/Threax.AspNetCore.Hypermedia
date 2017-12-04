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
        public static String GetPluralName(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(PluralNameAttribute.Name, out val);
            return val?.ToString();
        }
    }
}