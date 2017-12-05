using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    class ProfileTests
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, schema.GetKeyType().Name);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String modelIdType)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.ViewModels;
using {ns}.Models;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        public class Profile : IDisposable
        {{
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Profile()
            {{

            }}

            public void Dispose()
            {{
                mockup.Dispose();
            }}

            [Fact]
            void InputToEntity()
            {{
                var mapper = mockup.Get<IMapper>();
                var input = {Model}Tests.CreateInput();
                var entity = mapper.Map<{Model}Entity>(input);

                //Make sure the id does not copy over
                Assert.Equal(default({modelIdType}), entity.{Model}Id);
                AssertEqual(input, entity);
            }}

            [Fact]
            void EntityToView()
            {{
                var mapper = mockup.Get<IMapper>();
                var entity = {Model}Tests.CreateEntity();
                var view = mapper.Map<{Model}>(entity);

                Assert.Equal(entity.{Model}Id, view.{Model}Id);
                AssertEqual(entity, view);
            }}
        }}
    }}
}}";
        }
    }
}
