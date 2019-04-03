using Test.Controllers.Api;
using Test.InputModels;
using Test.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class LeftTests
    {
        public class Controller : IDisposable
        {
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Controller()
            {
                mockup.Add<LeftsController>(m => new LeftsController(m.Get<ILeftRepository>())
                {
                    ControllerContext = m.Get<ControllerContext>()
                });
            }

            public void Dispose()
            {
                mockup.Dispose();
            }

            [Fact]
            async Task List()
            {
                var totalItems = 3;

                var controller = mockup.Get<LeftsController>();

                for (var i = 0; i < totalItems; ++i)
                {
                    Assert.NotNull(await controller.Add(LeftTests.CreateInput()));
                }

                var query = new LeftQuery();
                var result = await controller.List(query);
                Assert.Equal(query.Limit, result.Limit);
                Assert.Equal(query.Offset, result.Offset);
                Assert.Equal(3, result.Total);
                Assert.NotEmpty(result.Items);
            }

            [Fact]
            async Task Get()
            {
                var totalItems = 3;

                var controller = mockup.Get<LeftsController>();

                for (var i = 0; i < totalItems; ++i)
                {
                    Assert.NotNull(await controller.Add(LeftTests.CreateInput()));
                }

                //Manually add the item we will look back up
                var lookup = await controller.Add(LeftTests.CreateInput());
                var result = await controller.Get(lookup.LeftId);
                Assert.NotNull(result);
            }

            [Fact]
            async Task Add()
            {
                var controller = mockup.Get<LeftsController>();

                var result = await controller.Add(LeftTests.CreateInput());
                Assert.NotNull(result);
            }

            [Fact]
            async Task Update()
            {
                var controller = mockup.Get<LeftsController>();

                var result = await controller.Add(LeftTests.CreateInput());
                Assert.NotNull(result);

                var updateResult = await controller.Update(result.LeftId, LeftTests.CreateInput());
                Assert.NotNull(updateResult);
            }

            [Fact]
            async Task Delete()
            {
                var controller = mockup.Get<LeftsController>();

                var result = await controller.Add(LeftTests.CreateInput());
                Assert.NotNull(result);

                var listResult = await controller.List(new LeftQuery());
                Assert.Equal(1, listResult.Total);

                await controller.Delete(result.LeftId);

                listResult = await controller.List(new LeftQuery());
                Assert.Equal(0, listResult.Total);
            }
        }
    }
}