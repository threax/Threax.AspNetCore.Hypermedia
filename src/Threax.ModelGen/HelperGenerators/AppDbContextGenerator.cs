using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class AppDbContextGenerator
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"Database/AppDbContext.{schema.Title}.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, Models);
        }

        public static String GetManyToManyEntityDbContextFileName(RelationshipSettings relationship)
        {
            return $"Database/AppDbContext.{relationship.LeftModelName}To{relationship.RightModelName}Entity.cs";
        }

        public static String GetManyToManyEntityDbContext(RelationshipSettings relationship, String ns)
        {
            String content = null;
            if (relationship.Kind == RelationKind.ManyToMany)
            {
                content = AppDbContextGenerator.Create(
                    ns,
                    $"Join{relationship.LeftModelName}To{relationship.RightModelName}",
                    $"Join{relationship.LeftModelName}To{relationship.RightModelName}");
            }
            return content;
        }

        public static String Create(String ns, String Model, String Models)
        {
            return
$@"using Microsoft.EntityFrameworkCore;

namespace {ns}.Database
{{
    public partial class AppDbContext
    {{
        public DbSet<{Model}Entity> {Models} {{ get; set; }}
    }}
}}
";
        }
    }
}
