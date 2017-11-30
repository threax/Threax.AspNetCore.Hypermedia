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

        public QueryableAttribute(bool required = false) : base(Name, required)
        {
        }

        public static bool IsQueryable(JsonSchema4 schema)
        {
            return schema.ExtensionData?.ContainsKey(Name) == true;
        }

        public static bool IsRequired(JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(Name, out val) == true)
            {
                bool? boolVal = val as bool?;
                if (boolVal != null)
                {
                    return boolVal == true;
                }
            }
            return false;
        }
    }
}
