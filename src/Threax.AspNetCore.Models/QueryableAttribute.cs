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
        private class Settings
        {
            public bool Required { get; set; }

            public bool ShowOnUi { get; set; }
        }

        private const String Name = "x-queryable";

        public QueryableAttribute(bool required = false, bool showOnUi = true) : base(Name, new Settings() { Required = required, ShowOnUi = showOnUi })
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
                Settings settings = val as Settings;
                if (settings != null)
                {
                    return settings.Required;
                }
            }
            return false;
        }

        public static bool ShowOnUi(JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(Name, out val) == true)
            {
                Settings settings = val as Settings;
                if (settings != null)
                {
                    return settings.ShowOnUi;
                }
            }
            return false;
        }
    }
}
