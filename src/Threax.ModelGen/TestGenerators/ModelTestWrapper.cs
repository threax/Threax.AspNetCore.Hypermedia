using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelTestWrapper
    {
        public static String Get(String ns, String modelName, String modelPluralName, JsonSchema4 schema)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);

            var equalAssertFunc = ModelTypeGenerator.Create(schema, modelPluralName, new ModelEqualityAssert(), schema, ns, ns);
            return Create(ns, Model, model, Models, models, equalAssertFunc);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String equalAssertFunc)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using {ns}.Models;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        private static Mockup SetupModel(this Mockup mockup)
        {{
            mockup.Add<I{Model}Repository>(m => new {Model}Repository(m.Get<AppDbContext>(), m.Get<IMapper>()));

            return mockup;
        }}

        public static {Model}Input CreateInput(String seed = """")
        {{
            return new {Model}Input()
            {{
                
            }};
        }}

        public static {Model}Entity CreateEntity(String seed = """", Guid? {model}Id = null)
        {{
            return new {Model}Entity()
            {{
                {Model}Id = {model}Id.HasValue ? {model}Id.Value : Guid.NewGuid(),
                
            }};
        }}

        {equalAssertFunc}
    }}
}}";
        }
    }
}
