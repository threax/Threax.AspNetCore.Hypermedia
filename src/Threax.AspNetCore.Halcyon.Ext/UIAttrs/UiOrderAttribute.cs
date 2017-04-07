using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    /// <summary>
    /// This class helps enforce the order of your properties in the json schema. It will add x-ui-order to the properties
    /// that it is applied to. There is no reason to supply an order value since it will be set to the line number you write
    /// the property onto. The order does not have to be sequential.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiOrderAttribute : JsonSchemaExtensionDataAttribute
    {
        /// <summary>
        /// Setup the ui order for this property.
        /// </summary>
        /// <param name="order">Order is optional, the default is the line number of the attribute.</param>
        public UiOrderAttribute([CallerLineNumber] int order = 0) : base("x-ui-order", order)
        {
        }
    }
}
