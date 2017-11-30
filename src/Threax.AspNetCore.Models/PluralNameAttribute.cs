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
        private const String Name = "x-plural-title";

        public PluralNameAttribute(String value) : base(Name, value)
        {
        }

        public static String GetValue(JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(Name, out val);
            return val?.ToString();
        }
    }
}