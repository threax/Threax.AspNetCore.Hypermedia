using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class QueryUserPartialGenerator
    {
        public static String GetQueryFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Query.cs";
        }

        public static String GetQuery(JsonSchema4 schema, String ns)
        {
            return Get(schema, ns + ".InputModels", "Query", schema.GetExtraNamespaces(StrConstants.FileNewline), ns);
        }

        private static String Get(JsonSchema4 schema, String modelNamespace, String modelType, String additionalNs, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            return Create(Model, model, modelNamespace, modelType, additionalNs, ns);
        }

        private static String Create(String Model, String model, String modelNamespace, String ModelType, String additionalNs, String ns)
        {
            return
$@"using System;
using {ns}.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;{additionalNs}

namespace {modelNamespace}
{{
    public partial class {Model}{ModelType}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{ModelType}.Generated for the generated code

        /// <summary>
        /// Populate an IQueryable. Does not apply the skip or limit.
        /// </summary>
        /// <param name=""query"">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public Task<IQueryable<{Model}Entity>> Create(IQueryable<{Model}Entity> query)
        {{
            if(CreateGenerated(ref query))
            {{
                //Customize query further
            }}

            return Task.FromResult(query);
        }}
    }}
}}";
        }
    }
}
