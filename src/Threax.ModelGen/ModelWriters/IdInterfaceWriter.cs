using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public class IdInterfaceWriter : InterfaceWriter
    {
        private JsonSchema4 schema;
        private String ns;

        public IdInterfaceWriter(bool hasCreated, bool hasModified, JsonSchema4 schema)
            :base(hasCreated, hasModified)
        {
            this.schema = schema;
        }

        public IdInterfaceWriter(JsonSchema4 schema) : this(false, false, schema)
        {
        }

        public override void StartNamespace(StringBuilder sb, string name)
        {
            this.ns = name;
            base.StartNamespace(sb, name);
        }

        public override void EndType(StringBuilder sb, String name, String pluralName)
        {
            base.EndType(sb, name, pluralName);

            String queryProps = ModelTypeGenerator.Create(schema, pluralName, new QueryPropertiesWriter(visibility: "", allowAttributes: false), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });

            sb.AppendLine(
$@"
    public partial interface I{name}Id
    {{
        Guid {name}Id {{ get; set; }}
    }}    

    public partial interface I{name}Query
    {{
        Guid? {name}Id {{ get; set; }}
        {queryProps}
    }}"
            );
        }
    }
}

