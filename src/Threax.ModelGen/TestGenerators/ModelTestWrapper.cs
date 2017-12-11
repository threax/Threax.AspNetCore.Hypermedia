using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

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
            String createArgs = "";

            var equalAssertFunc = ModelTypeGenerator.Create(schema, modelPluralName, new ModelEqualityAssert(), schema, ns, ns);

            createArgs = ModelTypeGenerator.Create(schema, modelPluralName, new ModelCreateArgs(), schema, ns, ns, p => p.CreateInputModel());
            var createInputFunc = ModelTypeGenerator.Create(schema, modelPluralName, new CreateInputModel(createArgs), schema, ns, ns, p => p.CreateInputModel());

            createArgs = ModelTypeGenerator.Create(schema, modelPluralName, new ModelCreateArgs(), schema, ns, ns, p => p.CreateEntity());
            var createEntityFunc = ModelTypeGenerator.Create(schema, modelPluralName, new CreateEntity(schema, createArgs), schema, ns, ns, p => p.CreateEntity());

            createArgs = ModelTypeGenerator.Create(schema, modelPluralName, new ModelCreateArgs(), schema, ns, ns, p => p.CreateViewModel());
            var createViewFunc = ModelTypeGenerator.Create(schema, modelPluralName, new CreateViewModel(schema, createArgs), schema, ns, ns, p => p.CreateViewModel());

            return Create(ns, Model, model, Models, models, equalAssertFunc, createInputFunc, createEntityFunc, createViewFunc);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String equalAssertFunc, String createInputFunc, String createEntityFunc, String createViewFunc)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using {ns}.Models;
using {ns}.ViewModels;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        private static Mockup SetupModel(this Mockup mockup)
        {{
            mockup.Add<I{Model}Repository>(m => new {Model}Repository(m.Get<AppDbContext>(), m.Get<IMapper>()));

            return mockup;
        }}

{createInputFunc}

{createEntityFunc}

{createViewFunc}

{equalAssertFunc}
    }}
}}";
        }
    }
}
