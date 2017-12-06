using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class IdInterfaceWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            var sb = new StringBuilder();
            bool allPropertiesIncluded = true;

            //Common writer
            var commonWriter = new InterfaceWriter(false, false)
            {
                WriteEndNamespace = false
            };
            sb.Append(ModelTypeGenerator.Create(schema, schema.GetPluralName(), commonWriter, ns, ns + ".Models",
                a =>
            {
                allPropertiesIncluded = allPropertiesIncluded && a.CreateInputModel() && a.CreateEntity() && a.CreateViewModel();
                return a.CreateInputModel() && a.CreateEntity() && a.CreateViewModel();
            }));

            if (!allPropertiesIncluded)
            {
                var individualPropertiesWriter = new SinglePropertyInterfaceWriter(false, false)
                {
                    WriteNamespace = false,
                    WriteUsings = false,
                };

                //Find which properties need to be broken into their own class
                foreach (var prop in schema.Properties.Values)
                {
                    if (!prop.CreateInputModel() || !prop.CreateEntity() || !prop.CreateViewModel())
                    {
                        individualPropertiesWriter.PropName = prop.Name;
                        sb.AppendLine();
                        sb.Append(ModelTypeGenerator.Create(schema, schema.GetPluralName(), individualPropertiesWriter, ns, ns, p => p == prop));
                    }
                }
            }

            //Id and query interface
            var queryProps = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new QueryPropertiesWriter(visibility: "", allowAttributes: false), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            sb.Append($@"
    public partial interface I{schema.Title}Id
    {{
        {schema.GetKeyType().Name} {schema.Title}Id {{ get; set; }}
    }}    

    public partial interface I{schema.Title}Query
    {{
        {schema.GetKeyType().GetTypeAsNullable()} {schema.Title}Id {{ get; set; }}
        {queryProps}
    }}
}}"
            );

            return sb.ToString();
        }

        public static IEnumerable<String> GetInterfaces(JsonSchema4 schema, bool includeId, Func<JsonProperty, bool> includePropertyInferaceForCb)
        {
            yield return $"I{schema.Title}";
            if (includeId)
            {
                yield return $"I{schema.Title}Id";
            }
            foreach(var prop in schema.Properties.Values)
            {
                if (includePropertyInferaceForCb(prop))
                {
                    yield return GetPropertyInterfaceName(schema.Title, prop.Name);
                }
            }
        }

        public static String GetPropertyInterfaceName(String modelName, String propertyName)
        {
            return $"I{modelName}_{propertyName}";
        }
    }
}

