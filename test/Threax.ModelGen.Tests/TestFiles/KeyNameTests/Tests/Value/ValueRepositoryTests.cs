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
                Assert.Equal<int>(4, dbContext.Values.Count());
                await repo.Delete(result.CrazyKey);
                Assert.Equal<int>(3, dbContext.Values.Count());
            }

            [Fact]
            async Task Get()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                var result = await repo.Add(ValueTests.CreateInput());
                Assert.Equal<int>(4, dbContext.Values.Count());
                var getResult = await repo.Get(result.CrazyKey);
                Assert.NotNull(getResult);
            }

            [Fact]
            async Task HasValuesEmpty()
            {
                var repo = mockup.Get<IValueRepository>();
                Assert.False(await repo.HasValues());
            }

            [Fact]
            async Task HasValues()
            {
                var repo = mockup.Get<IValueRepository>();
                await repo.AddRange(new ValueInput[] { ValueTests.CreateInput(), ValueTests.CreateInput(), ValueTests.CreateInput() });
                Assert.True(await repo.HasValues());
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
                var updateResult = await repo.Update(result.CrazyKey, ValueTests.CreateInput());
                Assert.NotNull(updateResult);
            }
        }
    }
}