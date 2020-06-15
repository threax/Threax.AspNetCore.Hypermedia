using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.ModelGen
{
    class ModelTypeGenerator
    {
        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, JsonSchema4 rootSchema, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null)
        {
            return Create(schema, pluralName, typeWriter, defaultNs, ns, allowPropertyCallback);
        }

        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null, IEnumerable<KeyValuePair<String, JsonProperty>> additionalProperties = null)
        {
            var sb = new StringBuilder();
            typeWriter.AddUsings(sb, defaultNs);
            typeWriter.StartNamespace(sb, ns);
            typeWriter.StartType(sb, schema.Title, pluralName);

            var prettyName = schema.Title;

            IEnumerable<KeyValuePair<String, JsonProperty>> props = schema.Properties;
            if(additionalProperties != null)
            {
                props = props.Concat(additionalProperties);
            }

            foreach (var propPair in props)
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
