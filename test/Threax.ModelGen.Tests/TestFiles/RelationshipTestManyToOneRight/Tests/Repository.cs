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
    public static partial class RightTests
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
                var repo = mockup.Get<IRightRepository>();
                var result = await repo.Add(RightTests.CreateInput());
                Assert.NotNull(result);
            }

            [Fact]
            async Task AddRange()
            {
                var repo = mockup.Get<IRightRepository>();
                await repo.AddRange(new RightInput[] { RightTests.CreateInput(), RightTests.CreateInput(), RightTests.CreateInput() });
            }

            [Fact]
            async Task Delete()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<IRightRepository>();
                await repo.AddRange(new RightInput[] { RightTests.CreateInput(), RightTests.CreateInput(), RightTests.CreateInput() });
                var result = await repo.Add(RightTests.CreateInput());
                Assert.Equal<int>(4, dbContext.Rights.Count());
                await repo.Delete(result.RightId);
                Assert.Equal<int>(3, dbContext.Rights.Count());
            }

            [Fact]
            async Task Get()
            {
                var dbContext = mockup.Get<AppDbContext>();
                var repo = mockup.Get<IRightRepository>();
                await repo.AddRange(new RightInput[] { RightTests.CreateInput(), RightTests.CreateInput(), RightTests.CreateInput() });
                var result = await repo.Add(RightTests.CreateInput());
                Assert.Equal<int>(4, dbContext.Rights.Count());
                var getResult = await repo.Get(result.RightId);
                Assert.NotNull(getResult);
            }

            [Fact]
            async Task HasRightsEmpty()
            {
                var repo = mockup.Get<IRightRepository>();
                Assert.False(await repo.HasRights());
            }

            [Fact]
            async Task HasRights()
            {
                var repo = mockup.Get<IRightRepository>();
                await repo.AddRange(new RightInput[] { RightTests.CreateInput(), RightTests.CreateInput(), RightTests.CreateInput() });
                Assert.True(await repo.HasRights());
            }

            [Fact]
            async Task List()
            {
                //This could be more complete
                var repo = mockup.Get<IRightRepository>();
                await repo.AddRange(new RightInput[] { RightTests.CreateInput(), RightTests.CreateInput(), RightTests.CreateInput() });
                var query = new RightQuery();
                var result = await repo.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }

            [Fact]
            async Task Update()
            {
                var repo = mockup.Get<IRightRepository>();
                var result = await repo.Add(RightTests.CreateInput());
                Assert.NotNull(result);
                var updateResult = await repo.Update(result.RightId, RightTests.CreateInput());
                Assert.NotNull(updateResult);
            }
        }
    }
}