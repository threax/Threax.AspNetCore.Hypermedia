using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelTestWrapper
    {
        public static String Get(String ns, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, model, Models, models);
        }

        private static String Create(String ns, String Model, String model, String Models, String models)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using System;
using Threax.AspNetCore.Tests;

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
    }}
}}";
        }
    }
}
