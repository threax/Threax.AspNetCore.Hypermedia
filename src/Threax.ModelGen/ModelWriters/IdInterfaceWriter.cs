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

        public override string StartNamespace(string name)
        {
            this.ns = name;
            return base.StartNamespace(name);
        }

        public override string EndType(String name, String pluralName)
        {
            String queryProps = ModelTypeGenerator.Create(schema, pluralName, new QueryPropertiesWriter(visibility: "", allowAttributes: false), schema, ns, ns, allowPropertyCallback: p =>
            {
                return QueryableAttribute.IsQueryable(p) == true;
            });

            return $@"
{base.EndType(name, pluralName)}

    public partial interface I{name}Id
    {{
        Guid {name}Id {{ get; set; }}
    }}    

    public partial interface I{name}Query
    {{
        Guid? {name}Id {{ get; set; }}

        {queryProps}
    }}";
        }
    }
}

