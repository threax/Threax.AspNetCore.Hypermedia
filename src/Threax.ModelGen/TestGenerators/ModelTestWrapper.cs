using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class ModelTestWrapper
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"{schema.Title}/{schema.Title}Tests.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);

            var repoMockup = "";
            if (schema.CreateRepository())
            {
                repoMockup = $"mockup.Add<I{Model}Repository>(m => new {Model}Repository(m.Get<AppDbContext>(), m.Get<AppMapper>()));";
            }

            return Create(ns, Model, model, Models, models, schema.GetExtraNamespaces(StrConstants.FileNewline), repoMockup);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String additionalNs, String repoMockup)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using {ns}.Models;
using {ns}.ViewModels;
using {ns}.Mappers;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;{additionalNs}

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        private static Mockup SetupModel(this Mockup mockup)
        {{
            {repoMockup}

            return mockup;
        }}
    }}
}}";
        }
    }
}
