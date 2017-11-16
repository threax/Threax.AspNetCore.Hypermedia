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
        [Fact(Skip = "Server Not Running")]
        public async Task EntryPoint()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
                var entryPoint = await entryPointInjector.Load();
                Assert.NotNull(entryPoint);
            }
        }

        [Fact(Skip = "Server Not Running")]
        public async Task ListThingies()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
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
        }

        [Fact(Skip = "Server Not Running")]
        public async Task AddThingyDocs()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
                var entryPoint = await entryPointInjector.Load();
                var addThingyDocs = await entryPoint.GetAddThingyDocs();
                Assert.Null(addThingyDocs.QuerySchema);
                Assert.NotNull(addThingyDocs);
                Assert.NotNull(addThingyDocs.RequestSchema);
                Assert.NotNull(addThingyDocs.ResponseSchema);
            }
        }

        [Fact(Skip = "Server Not Running")]
        public async Task AddThingy()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
                var entryPoint = await entryPointInjector.Load();
                var result = await entryPoint.AddThingy(new ThingyView()
                {
                    Name = "Name",
                });
                Assert.Equal("Name", result.Data.Name);
            }
        }

        [Fact(Skip = "Server Not Running")]
        public async Task RawResultTest()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
                var entryPoint = await entryPointInjector.Load();
                using (var rawResult = await entryPoint.ReturnActionResult())
                {
                    Assert.NotNull(rawResult);
                }
            }
        }
    }
}
