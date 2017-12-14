using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class RepositoryTests
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, NameGenerator.CreatePascal(schema.GetKeyName()));
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String ModelId)
        {
            return
$@"using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using {ns}.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{
        public class Repository : IDisposable
        {{
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Repository()
            {{

            }}

            public void Dispose()
            {{
                mockup.Dispose();
            }}

            [Fact]
            async Task Add()
            {{
                var repo = mockup.Get<I{Model}Repository>();
                var result = await repo.Add({Model}Tests.CreateInput());
                Assert.NotNull(result);
            }}

            [Fact]
            async Task AddRange()
            {{
                var repo = mockup.Get<I{Model}Repository>();
                await repo.AddRange(new {Model}Input[] {{ {Model}Tests.CreateInput(), {Model}Tests.CreateInput(), {Model}Tests.CreateInput() }});
            }}

            [Fact]
            async Task Delete()
            {{
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<I{Model}Repository>();
                await repo.AddRange(new {Model}Input[] {{ {Model}Tests.CreateInput(), {Model}Tests.CreateInput(), {Model}Tests.CreateInput() }});
                var result = await repo.Add({Model}Tests.CreateInput());
                Assert.Equal<int>(4, dbContext.{Models}.Count());
                await repo.Delete(result.{ModelId});
                Assert.Equal<int>(3, dbContext.{Models}.Count());
            }}

            [Fact]
            async Task Get()
            {{
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<I{Model}Repository>();
                await repo.AddRange(new {Model}Input[] {{ {Model}Tests.CreateInput(), {Model}Tests.CreateInput(), {Model}Tests.CreateInput() }});
                var result = await repo.Add({Model}Tests.CreateInput());
                Assert.Equal<int>(4, dbContext.{Models}.Count());
                var getResult = await repo.Get(result.{ModelId});
                Assert.NotNull(getResult);
            }}

            [Fact]
            async Task Has{Models}Empty()
            {{
                var repo = mockup.Get<I{Model}Repository>();
                Assert.False(await repo.Has{Models}());
            }}

            [Fact]
            async Task Has{Models}()
            {{
                var repo = mockup.Get<I{Model}Repository>();
                await repo.AddRange(new {Model}Input[] {{ {Model}Tests.CreateInput(), {Model}Tests.CreateInput(), {Model}Tests.CreateInput() }});
                Assert.True(await repo.Has{Models}());
            }}

            [Fact]
            async Task List()
            {{
                //This could be more complete
                var repo = mockup.Get<I{Model}Repository>();
                await repo.AddRange(new {Model}Input[] {{ {Model}Tests.CreateInput(), {Model}Tests.CreateInput(), {Model}Tests.CreateInput() }});
                var query = new {Model}Query();
                var result = await repo.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }}

            [Fact]
            async Task Update()
            {{
                var repo = mockup.Get<I{Model}Repository>();
                var result = await repo.Add({Model}Tests.CreateInput());
                Assert.NotNull(result);
                var updateResult = await repo.Update(result.{ModelId}, {Model}Tests.CreateInput());
                Assert.NotNull(updateResult);
            }}
        }}
    }}
}}";
        }
    }
}
