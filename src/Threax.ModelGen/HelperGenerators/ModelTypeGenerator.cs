using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Threax.ModelGen
{
    static class SBExt
    {
        public static void AppendLineWithContent(this StringBuilder sb, String content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                sb.AppendLine(content);
            }
        }
    }

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

        public static String Create(String name, String pluralName, ITypeWriter typeWriter, String defaultNs, String ns, String prettyName = null)
        {
            lastPropertyNames.Clear();

            var sb = new StringBuilder(typeWriter.AddUsings(defaultNs));
            sb.AppendLine();
            sb.AppendLineWithContent(typeWriter.StartNamespace(ns));

            if (!String.IsNullOrWhiteSpace(prettyName))
            {
                sb.AppendLineWithContent(typeWriter.AddTypeDisplay(NameGenerator.CreatePascal(prettyName)));
            }

            sb.AppendLineWithContent(typeWriter.StartType(name, pluralName));

            sb.AppendLineWithContent(typeWriter.EndType(name, pluralName));
            sb.Append(typeWriter.EndNamespace());

            return sb.ToString();
        }

        public static String Create(JsonSchema4 schema, String pluralName, ITypeWriter typeWriter, JsonSchema4 rootSchema, String defaultNs, String ns)
        {
            lastPropertyNames.Clear();

            var sb = new StringBuilder(typeWriter.AddUsings(defaultNs));
            sb.AppendLine();
            sb.AppendLineWithContent(typeWriter.StartNamespace(ns));
            if (!String.IsNullOrWhiteSpace(schema.Title))
            {
                sb.AppendLineWithContent(typeWriter.AddTypeDisplay(schema.Title)); //Probably not right
            }
            sb.Append(typeWriter.StartType(schema.Title, pluralName));

            var prettyName = schema.Title;

            foreach (var propPair in schema.Properties)
            {
                sb.AppendLine();
                sb.AppendLine();

                var propName = propPair.Key;
                var prop = propPair.Value;

                String propPrettyName = NameGenerator.CreatePretty(propName);

                if (prop.MaxLength.HasValue)
                {
                    string error = null; //Look this up somehow
                    if (String.IsNullOrWhiteSpace(error))
                    {
                        error = $"{propPrettyName} must be less than {prop.MaxLength} characters.";
                    }
                    sb.AppendLineWithContent(typeWriter.AddMaxLength(prop.MaxLength.Value, error));
                }

                if (prop.IsRequired)
                {
                    string error = null; //Look this up somehow
                    if (String.IsNullOrWhiteSpace(error))
                    {
                        error = $"{propPrettyName} must have a value.";
                    }
                    sb.AppendLineWithContent(typeWriter.AddRequired(error));
                }

                if (!String.IsNullOrWhiteSpace(prop.Title))
                {
                    sb.AppendLineWithContent(typeWriter.AddDisplay(prop.Title));
                }

                var pascalPropName = NameGenerator.CreatePascal(propName);
                lastPropertyNames.Add(pascalPropName);
                sb.AppendLineWithContent(typeWriter.CreateProperty(prop.GetClrType(), pascalPropName, prop.IsClrValueType()));
            }

            sb.AppendLineWithContent(typeWriter.EndType(schema.Title, pluralName));
            sb.Append(typeWriter.EndNamespace());

            return sb.ToString();
        }
    }
}
