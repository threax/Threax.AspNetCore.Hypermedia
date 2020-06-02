using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;
using Threax.NJsonSchema.Generation;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaCustomizerArgs
    {
        public SchemaCustomizerArgs(string name, PropertyInfo typeProperty, JsonProperty schemaProperty, JsonSchema4 schema, JsonSchemaGenerator schemaGenerator, Type type)
        {
            this.Name = name;
            this.TypeProperty = typeProperty;
            this.SchemaProperty = schemaProperty;
            this.Schema = schema;
            this.SchemaGenerator = schemaGenerator;
            this.Type = type;
        }

        public Type Type { get; set; }

        public PropertyInfo TypeProperty { get; private set; }

        public String Name { get; private set; }

        public JsonSchema4 Schema { get; private set; }

        public JsonProperty SchemaProperty { get; private set; }

        public JsonSchemaGenerator SchemaGenerator { get; private set; }
    }
}
