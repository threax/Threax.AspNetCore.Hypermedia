using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaJsonConverter : JsonConverter
    {
        private static readonly TypeInfo JsonSchema4TypeInfo = typeof(JsonSchema4).GetTypeInfo();

        public override bool CanConvert(Type objectType)
        {
            return JsonSchema4TypeInfo.IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Reading json schemas is not supported");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jsonSchema = value as JsonSchema4;
            //This single line will do the same thing, but the object is not correct, the actual code for this
            //function does what ToJson below does, but uses json.net to serialize the schema so the settings
            //match the rest of the system.
            //writer.WriteRawValue(jsonSchema.ToJson(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));
            JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(jsonSchema);
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, jsonSchema);
                var json = JsonSchemaReferenceUtilities.ConvertPropertyReferences(sw.ToString());
                writer.WriteRawValue(json);
            }
        }
    }
}
