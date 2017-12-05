//#define ServerRunning
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
#if ServerRunning
        [Fact]
#else
        [Fact(Skip = "Server Not Running")]
#endif
        public async Task EntryPoint()
        {
            using (var clientFactory = new DefaultHttpClientFactory())
            {
                var entryPointInjector = new EntryPointsInjector("http://localhost:65405/", clientFactory);
                var entryPoint = await entryPointInjector.Load();
                Assert.NotNull(entryPoint);
            }
        }

#if ServerRunning
        [Fact]
#else
        [Fact(Skip = "Server Not Running")]
#endif
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

#if ServerRunning
        [Fact]
#else
        [Fact(Skip = "Server Not Running")]
#endif
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

#if ServerRunning
        [Fact]
#else
        [Fact(Skip = "Server Not Running")]
#endif
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

#if ServerRunning
        [Fact]
#else
        [Fact(Skip = "Server Not Running")]
#endif
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
