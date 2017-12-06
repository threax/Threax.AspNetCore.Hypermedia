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
            bool allPropertiesIncluded = true;
            var commonWriter = new InterfaceWriter(false, false)
            {
                WriteEndNamespace = false
            };
            var common = ModelTypeGenerator.Create(schema, schema.GetPluralName(), commonWriter, ns, ns + ".Models",
                a =>
            {
                allPropertiesIncluded = allPropertiesIncluded && a.CreateInputModel() && a.CreateEntity() && a.CreateViewModel();
                return a.CreateInputModel() && a.CreateEntity() && a.CreateViewModel();
            });

            var queryProps = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new QueryPropertiesWriter(visibility: "", allowAttributes: false), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            return $@"{common}
    public partial interface I{schema.Title}Id
    {{
        {schema.GetKeyType().Name} {schema.Title}Id {{ get; set; }}
    }}    

    public partial interface I{schema.Title}Query
    {{
        {schema.GetKeyType().GetTypeAsNullable()} {schema.Title}Id {{ get; set; }}
        {queryProps}
    }}
}}";
        }
    }
}

