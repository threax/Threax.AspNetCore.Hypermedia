using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //Going with this line to generate schema json for now. In the past there were problems with this
            //not generating the correct json, which are discussed below. For now the following seems to be working.
            //It will make all the property names lowercase except the definitions, which will stay in their original
            //case, which is what we need, or else the definitions cannot be resolved.
            writer.WriteRawValue(jsonSchema.ToJson());

            //-------------------------------------------------------------------------------
            //This is the same as the line above since in the actual NJsonSchema source code the settings passed in are ignored.
            //At least this was true as of 8-23-2017. If in the future you have problems, try using this version first
            //to see if the json schema generator settings are honored.
            //writer.WriteRawValue(jsonSchema.ToJson(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            //This code will use the current json serializer settings, but will mess up the definitions by lower casing the property names there.
            //No way could be found to avoid this before it was discovered that ToJson appears to be correct, but leaving this in as reference
            //for when this inevitably breaks in the future.
            //JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(jsonSchema);
            //using (var sw = new StringWriter())
            //{
            //    serializer.Serialize(sw, jsonSchema);
            //    var json = JsonSchemaReferenceUtilities.ConvertPropertyReferences(sw.ToString());
            //    writer.WriteRawValue(json);
            //}
            //-------------------------------------------------------------------------------
        }
    }
}
