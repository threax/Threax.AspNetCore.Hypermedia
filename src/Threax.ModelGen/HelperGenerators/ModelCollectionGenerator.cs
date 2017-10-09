using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class ModelCollectionGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model, model);
        }

        private static String Create(String ns, String Model, String model)
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
    [HalSelfActionLink(typeof({Model}sController), nameof({Model}sController.List))]
    [HalActionLink(typeof({Model}sController), nameof({Model}sController.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof({Model}sController), nameof({Model}sController.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof({Model}sController), nameof({Model}sController.Add))]
    [DeclareHalLink(typeof({Model}sController), nameof({Model}sController.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof({Model}sController), nameof({Model}sController.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof({Model}sController), nameof({Model}sController.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof({Model}sController), nameof({Model}sController.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
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
