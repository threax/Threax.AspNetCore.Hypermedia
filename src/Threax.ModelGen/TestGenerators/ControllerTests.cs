﻿using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class ControllerTests
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"{schema.Title}/{schema.Title}ControllerTests.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, NameGenerator.CreatePascal(schema.GetKeyName()), schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String ModelId, String additionalNs)
        {
            return
$@"using {ns}.Controllers.Api;
using {ns}.InputModels;
using {ns}.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;{additionalNs}

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        public class Controller : IDisposable
        {{
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Controller()
            {{
                mockup.Add<{Models}Controller>(m => new {Models}Controller(m.Get<I{Model}Repository>())
                {{
                    ControllerContext = m.Get<ControllerContext>()
                }});
            }}

            public void Dispose()
            {{
                mockup.Dispose();
            }}

            [Fact]
            async Task List()
            {{
                var totalItems = 3;

                var controller = mockup.Get<{Models}Controller>();

                for (var i = 0; i < totalItems; ++i)
                {{
                    Assert.NotNull(await controller.Add({Model}Tests.CreateInput()));
                }}

                var query = new {Model}Query();
                var result = await controller.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }}

            [Fact]
            async Task Get()
            {{
                var totalItems = 3;

                var controller = mockup.Get<{Models}Controller>();

                for (var i = 0; i < totalItems; ++i)
                {{
                    Assert.NotNull(await controller.Add({Model}Tests.CreateInput()));
                }}

                //Manually add the item we will look back up
                var lookup = await controller.Add({Model}Tests.CreateInput());
                var result = await controller.Get(lookup.{ModelId});
                Assert.NotNull(result);
            }}

            [Fact]
            async Task Add()
            {{
                var controller = mockup.Get<{Models}Controller>();

                var result = await controller.Add({Model}Tests.CreateInput());
                Assert.NotNull(result);
            }}

            [Fact]
            async Task Update()
            {{
                var controller = mockup.Get<{Models}Controller>();

                var result = await controller.Add({Model}Tests.CreateInput());
                Assert.NotNull(result);

                var updateResult = await controller.Update(result.{ModelId}, {Model}Tests.CreateInput());
                Assert.NotNull(updateResult);
            }}

            [Fact]
            async Task Delete()
            {{
                var controller = mockup.Get<{Models}Controller>();

                var result = await controller.Add({Model}Tests.CreateInput());
                Assert.NotNull(result);

                var listResult = await controller.List(new {Model}Query());
                Assert.Equal(1, listResult.Total);

                await controller.Delete(result.{ModelId});

                listResult = await controller.List(new {Model}Query());
                Assert.Equal(0, listResult.Total);
            }}
        }}
    }}
}}";
        }
    }
}
