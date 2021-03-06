﻿using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class ProfileTests
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"{schema.Title}/{schema.Title}ProfileTests.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, schema.GetKeyType().Name, schema.GetKeyName(), schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String modelIdType, String ModelId, String additionalNs)
        {
            return
$@"using AutoMapper;
using {ns}.Database;
using {ns}.ViewModels;
using {ns}.Mappers;
using System;
using Threax.AspNetCore.Tests;
using Xunit;{additionalNs}

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
                var mapper = mockup.Get<AppMapper>();
                var input = {Model}Tests.CreateInput();
                var entity = mapper.Map{Model}(input, new {Model}Entity());

                //Make sure the id does not copy over
                Assert.Equal(default({modelIdType}), entity.{ModelId});
                AssertEqual(input, entity);
            }}

            [Fact]
            void EntityToView()
            {{
                var mapper = mockup.Get<AppMapper>();
                var entity = {Model}Tests.CreateEntity();
                var view = mapper.Map{Model}(entity, new {Model}());

                Assert.Equal(entity.{ModelId}, view.{ModelId});
                AssertEqual(entity, view);
            }}
        }}
    }}
}}";
        }
    }
}
