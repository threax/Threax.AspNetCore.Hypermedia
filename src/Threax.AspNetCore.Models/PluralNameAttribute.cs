using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : JsonSchemaExtensionDataAttribute
    {
        public const String Name = "x-plural-title";

        public PluralNameAttribute(String value) : base(Name, value)
        {
        }
    }
}
