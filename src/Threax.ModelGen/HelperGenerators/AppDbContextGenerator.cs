using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class AppDbContextGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model);
        }

        private static String Create(String ns, String Model)
        {
            return
$@"using Microsoft.EntityFrameworkCore;

namespace {ns}.Database
{{
    public partial class AppDbContext
    {{
        public DbSet<{Model}Entity> {Model}s {{ get; set; }}
    }}
}}
";
        }
    }
}
