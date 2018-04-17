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
            String ModelId, modelId;
            NameGenerator.CreatePascalAndCamel(schema.GetKeyName(), out ModelId, out modelId);

            return Create(ns, Model, model, Models, models, schema.GetKeyType().GetTypeAsNullable(), ModelId, modelId, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String nullableModelIdType, String ModelId, String modelId, String additionalNs)
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
using Threax.AspNetCore.Halcyon.Ext;{additionalNs}

namespace {ns}.ViewModels
{{
    public partial class {Model}Collection : PagedCollectionViewWithQuery<{Model}, {Model}Query>
    {{
        
    }}
}}";
        }

        public static String GetUserPartial(JsonSchema4 schema, String ns, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return CreatePartial(ns, Model, model, Models, models, generatedSuffix, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String CreatePartial(String ns, String Model, String model, String Models, String models, String generatedSuffix, String additionalNs)
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
using Threax.AspNetCore.Halcyon.Ext;{additionalNs}

namespace {ns}.ViewModels
{{
    [HalModel]
    [HalSelfActionLink(typeof({Models}Controller), nameof({Models}Controller.List))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Get), DocsOnly = true)] //This provides access to docs for showing items
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.List), DocsOnly = true)] //This provides docs for searching the list
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Update), DocsOnly = true)] //This provides access to docs for updating items if the ui has different modes
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Add))]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Next, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Previous, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.First, ResponseOnly = true)]
    [DeclareHalLink(typeof({Models}Controller), nameof({Models}Controller.List), PagedCollectionView<Object>.Rels.Last, ResponseOnly = true)]
    public partial class {Model}Collection
    {{
        public {Model}Collection({Model}Query query, int total, IEnumerable<{Model}> items) : base(query, total, items)
        {{
            
        }}

        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}Collection{generatedSuffix} for the generated code
    }}
}}";
        }
    }
}
