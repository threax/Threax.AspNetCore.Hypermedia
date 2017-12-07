using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute modifies the name of the controller for the ui. You should not include Controller in your
    /// name. So if you wanted the final controller to be AdminController pass Admin to this class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UiControllerNameAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-ui-controller-name";

        public UiControllerNameAttribute(String controllerName) : base(Name, controllerName)
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
