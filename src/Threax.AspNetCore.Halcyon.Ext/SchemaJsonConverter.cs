using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
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
            writer.WriteRawValue(jsonSchema.ToJson());
        }
    }
}
