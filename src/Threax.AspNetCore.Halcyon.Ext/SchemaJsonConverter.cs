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

        public override bool CanRead => false;

        public override bool CanWrite => true;

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
            //This would be the ideal way to handle things, but njsonschema is really unpredictable
            //writer.WriteRawValue(jsonSchema.ToJson());

            //-------------------------------------------------------------------------------
            //This is the same as the line above since in the actual NJsonSchema source code the settings passed in are ignored.
            //At least this was true as of 8-23-2017. If in the future you have problems, try using this version first
            //to see if the json schema generator settings are honored.
            //writer.WriteRawValue(jsonSchema.ToJson(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            //This method serializes the json schema to a jobject and then goes through all schemaReferencePath (njsonschema's name for $ref) jtokens
            //and updates them to match the camel case definitions that are actually written. It will make sure the definition isn't resolvable in the
            //document first before replacing it.
            //
            //This makes a pretty big assumption that the camel case definition is the same as the definition in the schemaReferencePath, since the 
            //definitions are c# classes and enums, which should be PascalCase then this should hold true. However, this could be a source of problems
            //potentially as well.
            //
            //This algo seems to run just as fast in the real world as the options above even though it does a lot of work.
            //-------------------------------------------------------------------------------
            JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(jsonSchema);
            var jObj = JObject.FromObject(jsonSchema, serializer);

            //Repair definition paths, this has to be post processed due to the way we are serializing
            foreach (var schemaReferencePath in jObj.SelectTokens("$..schemaReferencePath").OfType<JValue>())
            {
                //First make sure we can't actually find the definition
                var refPath = schemaReferencePath.Value.ToString();
                if (refPath[0] == '#') //If path starts with # it is in the document.
                {
                    var splitDef = refPath.Split('/');
                    if (splitDef.Length > 0) //If there is nothing to split, do nothing
                    {
                        //Build a JPath from the ref path, (starts with $ and uses . instead of / to separate items
                        var sb = new StringBuilder(refPath.Length);
                        sb.Append("$");
                        foreach (var item in splitDef.Skip(1))
                        {
                            sb.AppendFormat(".{0}", item);
                        }

                        //Check to see if we can find the definition on the original path, if so don't change it
                        var definition = jObj.SelectToken(sb.ToString());
                        if (definition == null)
                        {
                            //Didn't find definition, look for it with the last item in camelCase, to see if it got mangled
                            var index = sb.Length - splitDef[splitDef.Length - 1].Length;
                            var lowered = char.ToLowerInvariant(sb[index]); //Get the lowercase version of the first letter of the last path section.
                            sb[index] = lowered;
                            definition = jObj.SelectToken(sb.ToString());
                            if (definition == null)
                            {
                                throw new InvalidOperationException($"Could not find definition {refPath} in json schema in either pascal or camel case.");
                            }
                            //Update the ref path to use camelCase for the final string, reuse the builder
                            sb.Clear();
                            sb.Append(refPath);
                            sb[index] = lowered; //Replace the character again, in the original string
                            schemaReferencePath.Value = sb.ToString(); //Set the camel cased value back on the schemaReferencePath
                        }
                    }
                }
            }
            var json = JsonSchemaReferenceUtilities.ConvertPropertyReferences(jObj.ToString());
            writer.WriteRawValue(json);
        }
    }
}
