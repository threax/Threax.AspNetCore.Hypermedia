using NJsonSchema.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using NJsonSchema;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class EnumValueEnumNameGenerator : IEnumNameGenerator
    {
        public string Generate(int index, string name, object value, JsonSchema4 schema)
        {
            return value.ToString(); //Just return the value, name for the way we are using enums is pretty and has spaces.
        }
    }
}
