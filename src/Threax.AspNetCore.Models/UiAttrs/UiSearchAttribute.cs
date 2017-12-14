using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this attribute to specify that the property should be shown on auto generated
    /// search uis.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiSearchAttribute : JsonSchemaExtensionDataAttribute
    {
        public UiSearchAttribute() : base("x-ui-search", true)
        {
        }
    }
}
