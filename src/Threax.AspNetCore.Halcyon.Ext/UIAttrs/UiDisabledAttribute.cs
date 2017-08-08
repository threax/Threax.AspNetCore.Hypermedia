using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    /// <summary>
    /// This attribute sets a field to disabled by setting its x-ui-disabled attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiDisabledAttribute : JsonSchemaExtensionDataAttribute
    {
        public const String ExtensionName = "x-ui-disabled";

        public UiDisabledAttribute(bool disabled = true) : base(ExtensionName, disabled)
        {
        }
    }
}
