using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class ModelTestWrapper
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            String createArgs = "";

            var equalAssertFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelEqualityAssert(), schema, ns, ns);

            createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateInputModel());
            var createInputFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateInputModel(createArgs), schema, ns, ns, p => p.CreateInputModel());

            createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateEntity());
            var createEntityFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateEntity(schema, createArgs), schema, ns, ns, p => p.CreateEntity());

            createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateViewModel());
            var createViewFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateViewModel(schema, createArgs), schema, ns, ns, p => p.CreateViewModel());

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
