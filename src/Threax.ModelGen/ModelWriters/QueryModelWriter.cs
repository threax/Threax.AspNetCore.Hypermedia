﻿using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class QueryModelWriter
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Query.Generated.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            bool hasBase = false;

            var baseWriter = new QueryBaseModelWriter("Query", QueryPropertiesWriter.CreateAttributeBuilder())
            {
                InheritFrom = new String[] { "PagedCollectionQuery" }
            };
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseWriter, ns, ns + ".Database", allowPropertyCallback: p =>
            {
                if (p.IsQueryable())
                {
                    hasBase = hasBase | p.IsAbstractOnQuery();
                    return p.IsAbstractOnQuery();
                }
                return false;
            });

            var baseClassName = "PagedCollectionQuery";
            if (hasBase)
            {
                baseClassName = $"{BaseModelWriter.CreateBaseClassName(schema.Title, "Query")}";
                baseClass = $@"
{baseClass}
";
            }
            else
            {
                baseClass = "";
            }

            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            String queryProps = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new QueryPropertiesWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable() && !p.IsAbstractOnQuery();
            });
            String queryCreate = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new QueryCreateWriter(), schema, ns, ns, allowPropertyCallback: p =>
            {
                return p.IsQueryable();
            });
            return Create(ns, Model, model, Models, models, queryProps, queryCreate, schema.GetKeyType().GetTypeAsNullable(), baseClass, baseClassName, NameGenerator.CreatePascal(schema.GetKeyName()), schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String queryProps, String queryCreate, String nullableModelIdType, String baseClass, String baseClassName, String ModelId, String additionalNs)
        {
            return
$@"using Halcyon.HAL.Attributes;
using {ns}.Controllers;
using {ns}.Models;
using {ns}.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;{additionalNs}

namespace {ns}.InputModels
{{{baseClass}
    [HalModel]
    public partial class {Model}Query : {baseClassName}, I{Model}Query
    {{
        /// <summary>
        /// Lookup a {model} by id.
        /// </summary>
        public {nullableModelIdType} {ModelId} {{ get; set; }}

{queryProps}
        /// <summary>
        /// Populate an IQueryable for {models}. Does not apply the skip or limit. Will return
        /// true if the query should be modified or false if the entire query was built and should
        /// be left alone.
        /// </summary>
        /// <param name=""query"">The query to populate.</param>
        /// <returns>True if the query should continue to be built, false if it should be left alone.</returns>
        protected bool CreateGenerated(ref IQueryable<{Model}Entity> query)
        {{
            if ({ModelId} != null)
            {{
                query = query.Where(i => i.{ModelId} == {ModelId});
                return false;
            }}
            else
            {{
{queryCreate}                return true;
            }}
        }}
    }}
}}";
        }
    }
}
