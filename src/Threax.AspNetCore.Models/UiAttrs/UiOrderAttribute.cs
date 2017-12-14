using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This class helps enforce the order of your properties in the json schema. It will add x-ui-order to the properties
    /// that it is applied to. There is no reason to supply an order value since it will be set to the line number you write
    /// the property onto. The order does not have to be sequential.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiOrderAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-ui-order";

        /// <summary>
        /// Setup the ui order for this property.
        /// </summary>
        /// <param name="order">Order is optional, the default is the line number of the attribute.</param>
        /// <param name="offset">An offset to add to the calculated order value. Useful to try to keep order between multiple classes.</param>
        public UiOrderAttribute(int offset = 0, [CallerLineNumber] int order = 0) : base(Name, order + offset)
        {
        }
    }

    public static class OrderAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the order of the property. This will return null if there is no order.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static int? GetOrder(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(UiOrderAttribute.Name, out val);
            return (int?)val;
        }

        /// <summary>
        /// Set the order of this property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="order"></param>
        public static void SetOrder(this JsonProperty prop, int order)
        {
            prop.ExtensionData = prop.ExtensionData ?? new Dictionary<String, Object>();
            prop.ExtensionData[UiOrderAttribute.Name] = order;
        }
    }
}
