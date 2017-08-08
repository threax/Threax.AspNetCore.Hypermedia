using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This class adds extensions to json schema for code generation.
    /// </summary>
    public static class SchemaExtensions
    {
        private const String IsArrayExt = "x-is-array";
        private const string DataIsFormExt = "x-data-is-form";
        private const string RawResponseExt = "x-raw-response";

        /// <summary>
        /// Set that a particular field is an array or array-like value
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="value"></param>
        public static void SetIsArray(this JsonSchema4 schema, bool value)
        {
            SetExtension(schema, IsArrayExt, value);
        }

        /// <summary>
        /// Determine if a particular field is an array.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsArray(this JsonSchema4 schema)
        {
            return IsExtensionTrue(schema, IsArrayExt);
        }

        /// <summary>
        /// Set this to true to send the request data as form data instead of json.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="value"></param>
        public static void SetDataIsForm(this JsonSchema4 schema, bool value)
        {
            SetExtension(schema, DataIsFormExt, value);
        }

        /// <summary>
        /// This will be true if data should be sent as form data.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool DataIsForm(this JsonSchema4 schema)
        {
            return IsExtensionTrue(schema, DataIsFormExt);
        }

        /// <summary>
        /// Set this to true to mark the response as a raw http response instead of a halcyon json object.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="value"></param>
        public static void SetRawResponse(this JsonSchema4 schema, bool value)
        {
            SetExtension(schema, RawResponseExt, value);
        }

        /// <summary>
        /// This will be true if the link returns a raw response instead of a halcyon json object.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsRawResponse(this JsonSchema4 schema)
        {
            return IsExtensionTrue(schema, RawResponseExt);
        }

        /// <summary>
        /// Set an extension to value, will ensure everything is safe to do.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SetExtension(this JsonSchema4 schema, String key, bool value)
        {
            EnsureExtensionData(schema);
            schema.ExtensionData.Add(key, value);
        }

        /// <summary>
        /// Determine if an extension is set to true. This will only be true if extension data exists on the schema,
        /// the schema has the given extension key and that extension key's value is set to true.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExtensionTrue(this JsonSchema4 schema, String key)
        {
            object value;
            if (schema.ExtensionData != null && schema.ExtensionData.TryGetValue(key, out value))
            {
                bool? result = value as bool?;
                if (result != null && result.HasValue)
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
