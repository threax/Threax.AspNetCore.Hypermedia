using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ICustomJsonSerializer
    {
        void WriteJson(JsonWriter writer, JsonSerializer serializer);
    }

    public class CustomJsonSerializerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICustomJsonSerializer).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException($"Can only write json using the {nameof(CustomJsonSerializerConverter)}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ((ICustomJsonSerializer)value).WriteJson(writer, serializer);
        }
    }
}
