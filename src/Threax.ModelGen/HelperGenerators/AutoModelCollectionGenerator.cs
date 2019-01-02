using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class AutoModelCollectionGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);

            return Create(ns, Model, model, Models, models, schema.GetKeyType().GetTypeAsNullable(), schema.GetExtraNamespaces(StrConstants.FileNewline), false, true, false, null);
        }

        public static String GetUserPartial(JsonSchema4 schema, String ns, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, schema.GetKeyType().GetTypeAsNullable(), schema.GetExtraNamespaces(StrConstants.FileNewline), true, false, true, generatedSuffix);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String nullableModelIdType, String additionalNs, bool includeLinks, bool includeInheritance, bool includeConstructor, string generatedSuffix)
        {
            var result =
$@"using Halcyon.HAL.Attributes;
using {ns}.Controllers.Api;
using {ns}.Models;
using {ns}.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;{additionalNs}

namespace {ns}.ViewModels
{{";
            if (includeLinks)
            {
                result +=$@"
    [HalModel]
    [HalSelfActionLink(typeof({Models}Controller), nameof({Models}Controller.List))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Add))]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]";
            }

            result +=$@"
    public partial class {Model}Collection"; if (includeInheritance) { result  += $" : PagedCollectionViewWithQuery<{Model}, {Model}Query>"; }
            result +=$@"
    {{
        ";
            if (includeConstructor)
            {
                result +=
$@"public {Model}Collection({Model}Query query, int total, IEnumerable<{Model}> items) : base(query, total, items)
        {{
            
        }}";
            }

            if (generatedSuffix != null)
            {
                result += $@"

        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}Collection{generatedSuffix} for the generated code";
            }
            result += $@"
    }}
}}";

            return result;
        }
    }
}
