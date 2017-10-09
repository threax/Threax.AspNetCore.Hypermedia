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
    [HalSelfActionLink(CrudRels.List, typeof({Model}sController))]
    [HalActionLink(CrudRels.Get, typeof({Model}sController), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(CrudRels.List, typeof({Model}sController), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(CrudRels.Add, typeof({Model}sController))]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Next, CrudRels.List, typeof({Model}sController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Previous, CrudRels.List, typeof({Model}sController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.First, CrudRels.List, typeof({Model}sController), ResponseOnly = true)]
    [DeclareHalLink(PagedCollectionView<Object>.Rels.Last, CrudRels.List, typeof({Model}sController), ResponseOnly = true)]
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
