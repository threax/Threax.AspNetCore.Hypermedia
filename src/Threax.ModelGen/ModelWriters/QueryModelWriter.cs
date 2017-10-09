using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public static class QueryModelWriter
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
using {ns}.Controllers;
using {ns}.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace {ns}.InputModels
{{
    [HalModel]
    public class {Model}Query : PagedCollectionQuery, I{Model}Query
    {{
        /// <summary>
        /// Lookup a {model} by id.
        /// </summary>
        public Guid? {Model}Id {{ get; set; }}

        //Add any additional query parameters here

        /// <summary>
        /// Populate an IQueryable for {model}s. Does not apply the skip or limit.
        /// </summary>
        /// <param name=""query"">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public IQueryable<T> Create<T>(IQueryable<T> query) where T : I{Model}, I{Model}Id
        {{
            if ({Model}Id.HasValue)
            {{
                query = query.Where(i => i.{Model}Id == {Model}Id);
            }}
            else
            {{
                //Put additional model query conditions here, this way id lookup always works
            }}

            return query;
        }}
    }}
}}";
        }
    }
}
