using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    class JsonSchema4Converter : JsonConverter
    {
        private static readonly TypeInfo JsonSchema4TypeInfo = typeof(JsonSchema4).GetTypeInfo();

        public override bool CanConvert(Type objectType)
        {
            return JsonSchema4TypeInfo.IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var jObj = JObject.Load(reader);
            return jObj.ToObject<JsonSchema4>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Writing json schema not supported");
        }
    }
}
