using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Threax.AspNetCore.Models
{
    public static class OrderAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Makes use of the x-ui-order extension provided by the UiOrder class in the halcyon extensions, use that to specify order.
        /// </summary>
        internal const String Name = "x-ui-order";

        /// <summary>
        /// Get the order of the property. This will return null if there is no order.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static int? GetOrder(this JsonProperty prop)
        {
            Object val = null;
            prop.ExtensionData?.TryGetValue(Name, out val);
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
            prop.ExtensionData[Name] = order;
        }
    }
}