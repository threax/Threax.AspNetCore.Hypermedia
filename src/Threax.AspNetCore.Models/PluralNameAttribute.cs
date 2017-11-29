using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : JsonSchemaExtensionDataAttribute
    {
        public PluralNameAttribute(String value) : base("x-plural-title", value)
        {
        }
    }
}
