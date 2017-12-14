using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to alter the type of this a before sending the schema
    /// to the ui for processing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiTypeAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-ui-type";

        public UiTypeAttribute(String value) : base(Name, value)
        {
        }
    }

    public static class UiTypeAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the ui type of this property. Will return null if no ui type has been defined.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static String GetUiType(this JsonProperty prop)
        {
            Object val = null;
            if (prop.ExtensionData?.TryGetValue(UiTypeAttribute.Name, out val) == true)
            {
                return (String)val;
            }
            return null;
        }
    }
}
