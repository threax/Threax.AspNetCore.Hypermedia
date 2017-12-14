using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Define the label for null values for this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefineNullValueLabelAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-null-value-label";

        public DefineNullValueLabelAttribute() : this("None")
        {

        }

        public DefineNullValueLabelAttribute(String label) : base(Name, label)
        {
            
        }
    }

    public static class DefineNullValueLabelAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the null value label for a property. Will return null if there is no label.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static String GetNullValueLabel(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(DefineNullValueLabelAttribute.Name, out val);
            return val?.ToString();
        }
    }
}
