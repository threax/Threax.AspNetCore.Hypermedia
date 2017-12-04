using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Threax.ModelGen
{
    class ModelTypeGenerator
    {
        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, JsonSchema4 rootSchema, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null)
        {
            return Create(schema, pluralName, typeWriter, defaultNs, ns, allowPropertyCallback);
        }

        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null)
        {
            var sb = new StringBuilder();
            typeWriter.AddUsings(sb, defaultNs);
            typeWriter.StartNamespace(sb, ns);
            typeWriter.StartType(sb, schema.Title, pluralName);

            var prettyName = schema.Title;

            foreach (var propPair in schema.Properties)
            {
                if (allowPropertyCallback == null || allowPropertyCallback.Invoke(propPair.Value))
                {
                    var propName = propPair.Key;
                    var prop = propPair.Value;

                    typeWriter.CreateProperty(sb, NameGenerator.CreatePascal(propName), new SchemaWriterPropertyInfo(prop));
                }
            }

            typeWriter.EndType(sb, schema.Title, pluralName);
            typeWriter.EndNamespace(sb);

            return sb.ToString();
        }
    }
}
