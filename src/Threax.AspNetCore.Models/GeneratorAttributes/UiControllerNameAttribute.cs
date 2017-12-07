using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UiControllerNameAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-ui-controller-name";

        public UiControllerNameAttribute(String label) : base(Name, label)
        {

        }
    }

    public static class DefineUiControllerAttributeJsonSchemaExtensions
    {
        public static String GetUiControllerName(this JsonSchema4 prop)
        {
            Object val = null;
            if(prop.ExtensionData?.TryGetValue(UiControllerNameAttribute.Name, out val) == true)
            {
                return val?.ToString();
            }
            return "Home";
        }
    }
}
