using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Threax.ModelGen
{
    class ModelTypeGenerator
    {
        private static List<String> lastPropertyNames = new List<string>();

        public static IEnumerable<String> LastPropertyNames
        {
            get
            {
                return lastPropertyNames;
            }
        }

        //public static String Create(String name, String pluralName, ITypeWriter typeWriter, String defaultNs, String ns)
        //{
        //    lastPropertyNames.Clear();

        //    var sb = new StringBuilder();
        //    typeWriter.AddUsings(sb, defaultNs);
        //    typeWriter.StartNamespace(sb, ns);

        //    typeWriter.StartType(sb, name, pluralName);

        //    typeWriter.EndType(sb, name, pluralName);
        //    typeWriter.EndNamespace(sb);

        //    return sb.ToString();
        //}

        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, JsonSchema4 rootSchema, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null)
        {
            return Create(schema, pluralName, typeWriter, defaultNs, ns, allowPropertyCallback);
        }

        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, String defaultNs, String ns, Func<JsonProperty, bool> allowPropertyCallback = null)
        {
            lastPropertyNames.Clear();

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

                    var propPrettyName = NameGenerator.CreatePretty(propName);
                    var pascalPropName = NameGenerator.CreatePascal(propName);
                    lastPropertyNames.Add(pascalPropName);
                    typeWriter.CreateProperty(sb, pascalPropName, new SchemaWriterPropertyInfo(prop));
                }
            }

            typeWriter.EndType(sb, schema.Title, pluralName);
            typeWriter.EndNamespace(sb);

            return sb.ToString();
        }
    }
}
