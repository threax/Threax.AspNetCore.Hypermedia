using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public static class SchemaCustomizations
    {
        private const String ArrayExtensionName = "x-is-array";

        public static void SetIsArray(this JsonSchema4 schema, bool isArray)
        {
            EnsureExtensionData(schema);

            schema.ExtensionData.Add(ArrayExtensionName, isArray);
        }

        public static bool IsArray(this JsonSchema4 schema)
        {
            object value;
            if (schema.ExtensionData != null && schema.ExtensionData.TryGetValue(ArrayExtensionName, out value))
            {
                bool? result = value as bool?;
                if (result.HasValue)
                {
                    return result.Value;
                }
            }
            return false;
        }

        private static void EnsureExtensionData(JsonSchema4 schema)
        {
            if (schema.ExtensionData == null)
            {
                schema.ExtensionData = new Dictionary<String, Object>();
            }
        }
    }
}
