using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class QueryModelWriter
    {
        public static String Get(String ns, String modelName, String modelPluralName, JsonSchema4 schema)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            String queryProps = ModelTypeGenerator.Create(schema, modelPluralName, new QueryPropertiesWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            String queryCreate = ModelTypeGenerator.Create(schema, modelPluralName, new QueryCreateWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            return Create(ns, Model, model, Models, models, queryProps, queryCreate);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String queryProps, String queryCreate)
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
using Threax.AspNetCore.Halcyon.Ext.UIAttrs;
using System.ComponentModel.DataAnnotations;

namespace {ns}.InputModels
{{
    [HalModel]
    public partial class {Model}Query : PagedCollectionQuery, I{Model}Query
    {{
        /// <summary>
        /// Lookup a {model} by id.
        /// </summary>
        public Guid? {Model}Id {{ get; set; }}

{queryProps}
        /// <summary>
        /// Populate an IQueryable for {models}. Does not apply the skip or limit.
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
{queryCreate}                OnCreate(ref query);
            }}

            return query;
        }}

        partial void OnCreate<T>(ref IQueryable<T> query);
    }}
}}";
        }
    }
}
