using Test.Controllers.Api;
using Test.InputModels;
using Test.Repository;
using Test.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class ValueTests
    {
        public class Controller : IDisposable
        {
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Controller()
            {
                mockup.Add<ValuesController>(m => new ValuesController(m.Get<IValueRepository>())
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

                var controller = mockup.Get<ValuesController>();

                for (var i = 0; i < totalItems; ++i)
                {
                    Assert.NotNull(await controller.Add(ValueTests.CreateInput()));
                }

                var query = new ValueQuery();
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

                var controller = mockup.Get<ValuesController>();

                for (var i = 0; i < totalItems; ++i)
                {
                    Assert.NotNull(await controller.Add(ValueTests.CreateInput()));
                }

                //Manually add the item we will look back up
                var lookup = await controller.Add(ValueTests.CreateInput());
                var result = await controller.Get(lookup.ValueId);
                Assert.NotNull(result);
            }

            [Fact]
            async Task Add()
            {
                var controller = mockup.Get<ValuesController>();

                var result = await controller.Add(ValueTests.CreateInput());
                Assert.NotNull(result);
            }

            [Fact]
            async Task Update()
            {
                var controller = mockup.Get<ValuesController>();

                var result = await controller.Add(ValueTests.CreateInput());
                Assert.NotNull(result);

                var updateResult = await controller.Update(result.ValueId, ValueTests.CreateInput());
                Assert.NotNull(updateResult);
            }

            [Fact]
            async Task Delete()
            {
                var controller = mockup.Get<ValuesController>();

                var result = await controller.Add(ValueTests.CreateInput());
                Assert.NotNull(result);

                var listResult = await controller.List(new ValueQuery());
                Assert.Equal(1, listResult.Total);

                await controller.Delete(result.ValueId);

                listResult = await controller.List(new ValueQuery());
                Assert.Equal(0, listResult.Total);
            }
        }
    }
}