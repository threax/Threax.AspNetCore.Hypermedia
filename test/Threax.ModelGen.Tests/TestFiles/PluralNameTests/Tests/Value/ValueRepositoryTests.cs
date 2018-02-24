using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class ValueTests
    {
        public class Repository : IDisposable
        {
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Repository()
            {

            }

            public void Dispose()
            {
                mockup.Dispose();
            }

            [Fact]
            async Task Add()
            {
                var repo = mockup.Get<IValueRepository>();
                var result = await repo.Add(ValueTests.CreateInput());
                Assert.NotNull(result);
            }

            [Fact]
            async Task AddRange()
            {
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
            }

            [Fact]
            async Task Delete()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                var result = await repo.Add(ValueTests.CreateInput());
                Assert.Equal<int>(4, dbContext.LotsaValues.Count());
                await repo.Delete(result.ValueId);
                Assert.Equal<int>(3, dbContext.LotsaValues.Count());
            }

            [Fact]
            async Task Get()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                var result = await repo.Add(ValueTests.CreateInput());
                Assert.Equal<int>(4, dbContext.LotsaValues.Count());
                var getResult = await repo.Get(result.ValueId);
                Assert.NotNull(getResult);
            }

            [Fact]
            async Task HasLotsaValuesEmpty()
            {
                var repo = mockup.Get<IValueRepository>();
                Assert.False(await repo.HasLotsaValues());
            }

            [Fact]
            async Task HasLotsaValues()
            {
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                Assert.True(await repo.HasLotsaValues());
            }

            [Fact]
            async Task List()
            {
                //This could be more complete
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                var query = new ValueQuery();
                var result = await repo.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }

            [Fact]
            async Task Update()
            {
                var repo = mockup.Get<IValueRepository>();
                var result = await repo.Add(ValueTests.CreateInput());
                Assert.NotNull(result);
                var updateResult = await repo.Update(result.ValueId, ValueTests.CreateInput());
                Assert.NotNull(updateResult);
            }
        }
    }
}