using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to alter the type of this a before sending the schema
    /// to the ui for processing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiTypeAttribute : JsonSchemaExtensionDataAttribute
    {
        public UiTypeAttribute(String value) : base("x-ui-type", value)
        {
        }
    }
}
