using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this attribute to rename a class on the output ui. Usually the title is the
    /// class name, but this will alter that property before sending the schema to the ui
    /// for display.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UiTitleAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-ui-title";

        public UiTitleAttribute(String title) : base(Name, title)
        {
        }
    }

    public static class UiTitleAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the ui title of this property. Will return null if no ui title has been defined.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static String GetUiTitle(this JsonProperty prop)
        {
            Object val = null;
            if (prop.ExtensionData?.TryGetValue(UiTitleAttribute.Name, out val) == true)
            {
                return (String)val;
            }
            return null;
        }
    }
}
