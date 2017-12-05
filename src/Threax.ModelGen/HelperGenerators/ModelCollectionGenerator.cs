using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    static class ModelCollectionGenerator
    {
        class QueryPropWriter : AbstractTypeWriter
        {
            public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
            {
                sb.AppendLine(
$@"        public {info.ClrType}{QueryPropertiesWriter.CreateQueryNullable(info)} {name}
        {{
            get {{ return query.{name}; }}
            set {{ query.{name} = value; }}
        }}"
                );
                sb.AppendLine();
            }
        }

        class QueryCustomizerWriter : AbstractTypeWriter
        {
            public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
            {
                if (info.IsRequiredInQuery)
                {
                    sb.AppendLine($@"            queryString.AppendItem(""{NameGenerator.CreateCamel(name)}"", {name}.ToString());");
                }
                else
                {
                    sb.AppendLine(
    $@"            if ({name} != null)
            {{
                queryString.AppendItem(""{NameGenerator.CreateCamel(name)}"", {name}.ToString());
            }}"
                    );
                }
                sb.AppendLine();
            }
        }

        public static String Get(String ns, String modelName, String modelPluralName, JsonSchema4 schema)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            String queryProps = ModelTypeGenerator.Create(schema, modelPluralName, new QueryPropWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            String customizer = ModelTypeGenerator.Create(schema, modelPluralName, new QueryCustomizerWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            return Create(ns, Model, model, Models, models, queryProps, customizer, schema.GetKeyType().GetTypeAsNullable());
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String queryProps, String customizer, String nullableModelIdType)
        {
            return
$@"using Halcyon.HAL.Attributes;
using {ns}.Controllers.Api;
using {ns}.Models;
using {ns}.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace {ns}.ViewModels
{{
    public partial class {Model}Collection : PagedCollectionView<{Model}>, I{Model}Query
    {{
        private {Model}Query query;

        public {nullableModelIdType} {Model}Id
        {{
            get {{ return query.{Model}Id; }}
            set {{ query.{Model}Id = value; }}
        }}

{queryProps}        protected override void AddCustomQuery(string rel, QueryStringBuilder queryString)
        {{
            if ({Model}Id != null)
            {{
                queryString.AppendItem(""{model}Id"", {Model}Id.ToString());
            }}

{customizer}
            OnAddCustomQuery(rel, queryString);

            base.AddCustomQuery(rel, queryString);
        }}

        partial void OnAddCustomQuery(String rel, QueryStringBuilder queryString);
    }}
}}";
        }

        public static String GetUserPartial(String ns, String modelName, String modelPluralName, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return CreatePartial(ns, Model, model, Models, models, generatedSuffix);
        }

        private static String CreatePartial(String ns, String Model, String model, String Models, String models, String generatedSuffix)
        {
            return
$@"using Halcyon.HAL.Attributes;
using {ns}.Controllers.Api;
using {ns}.Models;
using {ns}.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace {ns}.ViewModels
{{
    [HalModel]
    [HalSelfActionLink(typeof({Models}Controller), nameof({Models}Controller.List))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Add))]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    public partial class {Model}Collection
    {{
        public {Model}Collection({Model}Query query, int total, IEnumerable<{Model}> items) : base(query, total, items)
        {{
            this.query = query;
        }}

        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}Collection{generatedSuffix} for the generated code
    }}
}}";
        }
    }
}
