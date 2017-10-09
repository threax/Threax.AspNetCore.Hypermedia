using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class ModelCollectionGenerator
    {
        public static String Get(String ns, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, model, Models, models);
        }

        private static String Create(String ns, String Model, String model, String Models, String models)
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
    public partial class {Model}Collection : PagedCollectionView<{Model}>, I{Model}Query
    {{
        public {Model}Collection({Model}Query query, int total, IEnumerable<{Model}> items) : base(query, total, items)
        {{
            this.{Model}Id = query.{Model}Id;
        }}

        public Guid? {Model}Id {{ get; set; }}

        protected override void AddCustomQuery(string rel, QueryStringBuilder queryString)
        {{
            if ({Model}Id.HasValue)
            {{
                queryString.AppendItem(""{model}Id"", {Model}Id.Value.ToString());
            }}

            base.AddCustomQuery(rel, queryString);
        }}
    }}
}}";
        }
    }
}
