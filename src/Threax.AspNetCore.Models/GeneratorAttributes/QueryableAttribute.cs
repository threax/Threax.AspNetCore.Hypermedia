using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Mark a property with this attribute to include it in the query class. You can also specify if the property should 
    /// show up on a query ui (default true) and whether or not the property is required in the query (default false).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryableAttribute : JsonSchemaExtensionDataAttribute
    {
        internal class Settings
        {
            public bool Required { get; set; }

            public bool ShowOnUi { get; set; }
        }

        internal const String Name = "x-queryable";

        public QueryableAttribute(bool required = false, bool showOnUi = true) : base(Name, new Settings() { Required = required, ShowOnUi = showOnUi })
        {
        }
    }

    public static class QueryableAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// This will be true if the property should be included in query objects.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsQueryable(this JsonSchema4 schema)
        {
            return schema.ExtensionData?.ContainsKey(QueryableAttribute.Name) == true;
        }

        /// <summary>
        /// This will be true if the property is required in the query.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsQueryableRequired(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(QueryableAttribute.Name, out val) == true)
            {
                var settings = val as QueryableAttribute.Settings;
                if (settings != null)
                {
                    return settings.Required;
                }
            }
            return false;
        }

        /// <summary>
        /// This will be true if the property should be shown on the ui.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsQueryableShowOnUi(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(QueryableAttribute.Name, out val) == true)
            {
                var settings = val as QueryableAttribute.Settings;
                if (settings != null)
                {
                    return settings.ShowOnUi;
                }
            }
            return false;
        }
    }
}
