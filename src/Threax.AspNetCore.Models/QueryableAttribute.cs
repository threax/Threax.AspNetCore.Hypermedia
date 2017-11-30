using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryableAttribute : JsonSchemaExtensionDataAttribute
    {
        private const String Name = "x-queryable";

        public QueryableAttribute(bool value = true) : base(Name, value)
        {
        }

        public static bool? GetValue(JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(Name, out val);
            return (bool?)val;
        }
    }
}
