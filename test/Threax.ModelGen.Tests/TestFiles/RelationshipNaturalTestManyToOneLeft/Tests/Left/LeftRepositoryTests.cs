using Test.Database;
using Test.InputModels;
using Test.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class LeftTests
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
                var repo = mockup.Get<ILeftRepository>();
                var result = await repo.Add(LeftTests.CreateInput());
                Assert.NotNull(result);
            }

            [Fact]
            async Task AddRange()
            {
                var repo = mockup.Get<ILeftRepository>();
                await repo.AddRange(new LeftInput[] { LeftTests.CreateInput(), LeftTests.CreateInput(), LeftTests.CreateInput() });
            }

            [Fact]
            async Task Delete()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<ILeftRepository>();
                await repo.AddRange(new LeftInput[] { LeftTests.CreateInput(), LeftTests.CreateInput(), LeftTests.CreateInput() });
                var result = await repo.Add(LeftTests.CreateInput());
                Assert.Equal<int>(4, dbContext.Lefts.Count());
                await repo.Delete(result.LeftId);
                Assert.Equal<int>(3, dbContext.Lefts.Count());
            }

            [Fact]
            async Task Get()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<ILeftRepository>();
                await repo.AddRange(new LeftInput[] { LeftTests.CreateInput(), LeftTests.CreateInput(), LeftTests.CreateInput() });
                var result = await repo.Add(LeftTests.CreateInput());
                Assert.Equal<int>(4, dbContext.Lefts.Count());
                var getResult = await repo.Get(result.LeftId);
                Assert.NotNull(getResult);
            }

            [Fact]
            async Task HasLeftsEmpty()
            {
                var repo = mockup.Get<ILeftRepository>();
                Assert.False(await repo.HasLefts());
            }

            [Fact]
            async Task HasLefts()
            {
                var repo = mockup.Get<ILeftRepository>();
                await repo.AddRange(new LeftInput[] { LeftTests.CreateInput(), LeftTests.CreateInput(), LeftTests.CreateInput() });
                Assert.True(await repo.HasLefts());
            }

            [Fact]
            async Task List()
            {
                //This could be more complete
                var repo = mockup.Get<ILeftRepository>();
                await repo.AddRange(new LeftInput[] { LeftTests.CreateInput(), LeftTests.CreateInput(), LeftTests.CreateInput() });
                var query = new LeftQuery();
                var result = await repo.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }

            [Fact]
            async Task Update()
            {
                var repo = mockup.Get<ILeftRepository>();
                var result = await repo.Add(LeftTests.CreateInput());
                Assert.NotNull(result);
                var updateResult = await repo.Update(result.LeftId, LeftTests.CreateInput());
                Assert.NotNull(updateResult);
            }
        }
    }
}