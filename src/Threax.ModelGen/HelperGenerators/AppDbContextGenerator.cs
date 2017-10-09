using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class AppDbContextGenerator
    {
        public static String Get(String ns, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, Models);
        }

        private static String Create(String ns, String Model, String Models)
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
