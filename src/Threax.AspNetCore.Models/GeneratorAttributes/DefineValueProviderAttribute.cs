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
    public class DefineValueProviderAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-value-provider";

        public DefineValueProviderAttribute(Type t) : base(Name, t.FullName)
        {
            
        }
    }

    public static class DefineValueProviderAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the type of the value provider for this property, will be null if no value provider is defined.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static String GetValueProviderType(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(DefineValueProviderAttribute.Name, out val);
            return val?.ToString();
        }
    }
}
