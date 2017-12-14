using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this attribute to rename a class on the output ui. Usually the title is the
    /// class name, but this will alter that property before sending the schema to the ui
    /// for display.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UiTitleAttribute : JsonSchemaExtensionDataAttribute
    {
        public UiTitleAttribute(String title) : base("x-ui-title", title)
        {
        }
    }
}
