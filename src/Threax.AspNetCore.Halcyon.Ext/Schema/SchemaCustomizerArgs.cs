using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NJsonSchema;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaCustomizerArgs
    {
        public SchemaCustomizerArgs(string name, PropertyInfo typeProperty, JsonProperty schemaProperty, JsonSchema4 schema)
        {
            this.Name = name;
            this.TypeProperty = typeProperty;
            this.SchemaProperty = schemaProperty;
            this.Schema = schema;
        }

        public PropertyInfo TypeProperty { get; set; }

        public String Name { get; set; }

        public JsonSchema4 Schema { get; set; }

        public JsonProperty SchemaProperty { get; set; }
    }
}
