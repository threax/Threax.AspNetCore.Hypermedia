using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Do not generate an input model for this class or do not include the marked property in the input model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NoUiAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-ui";

        public NoUiAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoUiAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an input model.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateUi(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoUiAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
