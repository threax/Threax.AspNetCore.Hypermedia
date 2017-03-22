using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    public class UiTypeAttribute : JsonSchemaExtensionDataAttribute
    {
        public UiTypeAttribute(object value) : base("x-ui-type", value)
        {
        }
    }
}
