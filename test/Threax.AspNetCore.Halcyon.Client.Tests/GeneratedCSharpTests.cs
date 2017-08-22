using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestHalcyonApi.ServiceClient;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Client.Tests
{
    public class GeneratedCSharpTests
    {
        [Fact]
        public async Task EntryPoint()
        {
            var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", new DefaultHttpClientFactory());
            var entryPoint = await entryPointInjector.Load();
            Assert.NotNull(entryPoint);
        }

        [Fact]
        public async Task ListThingies()
        {
            var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", new DefaultHttpClientFactory());
            var entryPoint = await entryPointInjector.Load();
            var thingies = await entryPoint.ListThingies(new PagedCollectionQuery()
            {
                Limit = 15,
                Offset = 0
            });
            Assert.NotNull(thingies);

            var items = thingies.Items;
            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task AddThingyDocs()
        {
            var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", new DefaultHttpClientFactory());
            var entryPoint = await entryPointInjector.Load();
            var addThingyDocs = await entryPoint.GetAddThingyDocs();
            Assert.Null(addThingyDocs.QuerySchema);
            Assert.NotNull(addThingyDocs);
            Assert.NotNull(addThingyDocs.RequestSchema);
            Assert.NotNull(addThingyDocs.ResponseSchema);
        }

        [Fact]
        public async Task RawResultTest()
        {
            var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", new DefaultHttpClientFactory());
            var entryPoint = await entryPointInjector.Load();
            using (var rawResult = await entryPoint.ReturnActionResult())
            {
                Assert.NotNull(rawResult);
            }
        }
    }
}
