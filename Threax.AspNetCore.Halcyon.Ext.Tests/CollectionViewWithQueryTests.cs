using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class CollectionViewWithQueryTests
    {
        private Mockup mockup = new Mockup().SetupGlobal();

        [Fact]
        public async Task Schema()
        {
            var generator = mockup.Get<EndpointDocJsonSchemaGenerator>();
            var schema = await generator.GenerateAsync(typeof(TestCollection));
            var filename = $"{nameof(Schema)}.json";
            FileUtils.WriteTestFile(this.GetType(), filename, schema.ToJson());
            var expected = FileUtils.ReadTestFile(this.GetType(), filename);
            Assert.Equal(expected, schema.ToJson());
        }

        [Fact]
        public void RequestData()
        {
            TestCollection collection = CreateCollection();
            var queryStringBuilder = new RequestDataBuilder();
            collection.AddQuery(HalSelfActionLinkAttribute.SelfRelName, queryStringBuilder);

            var serializer = mockup.Get<JsonSerializer>();
            using (var stream = new MemoryStream())
            {
                using (var writer = new JsonTextWriter(new StreamWriter(stream, Encoding.Default, 4096, true)))
                {
                    serializer.Serialize(writer, queryStringBuilder.Data);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var filename = $"{nameof(RequestData)}.json";
                    FileUtils.WriteTestFile(this.GetType(), filename, json);
                    var expected = FileUtils.ReadTestFile(this.GetType(), filename);
                    Assert.Equal(expected, json);
                }
            }
        }

        [Fact]
        public void RequestDataWithCollection()
        {
            var queryStringBuilder = new RequestDataBuilder();
            queryStringBuilder.AppendItem("ArrayOfThings", 1);
            queryStringBuilder.AppendItem("ArrayOfThings", 2);
            queryStringBuilder.AppendItem("ArrayOfThings", 3);
            queryStringBuilder.AppendItem("SingleThing", "hi");

            var serializer = mockup.Get<JsonSerializer>();
            using (var stream = new MemoryStream())
            {
                using (var writer = new JsonTextWriter(new StreamWriter(stream, Encoding.Default, 4096, true)))
                {
                    serializer.Serialize(writer, queryStringBuilder.Data);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var filename = $"{nameof(RequestDataWithCollection)}.json";
                    FileUtils.WriteTestFile(this.GetType(), filename, json);
                    var expected = FileUtils.ReadTestFile(this.GetType(), filename);
                    Assert.Equal(expected, json);
                }
            }
        }

        [Fact]
        public void Json()
        {
            TestCollection collection = CreateCollection();

            var serializer = mockup.Get<JsonSerializer>();
            using (var stream = new MemoryStream())
            {
                using (var writer = new JsonTextWriter(new StreamWriter(stream, Encoding.Default, 4096, true)))
                {
                    serializer.Serialize(writer, collection);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var filename = $"{nameof(Json)}.json";
                    FileUtils.WriteTestFile(this.GetType(), filename, json);
                    var expected = FileUtils.ReadTestFile(this.GetType(), filename);
                    Assert.Equal(expected, json);
                }
            }
        }

        private static TestCollection CreateCollection()
        {
            var query = new TestQuery()
            {
                AnInt = 1,
                PRETest = "Woot"
            };

            var items = new List<Test>
            {
                new Test()
                {
                    IntVal = 1,
                    StringVal = "Da String"
                },
                new Test()
                {
                    IntVal = -76,
                    StringVal = "Another String"
                },
                new Test()
                {
                    IntVal = 1233,
                    StringVal = "Tacos"
                }
            };

            var collection = new TestCollection(query, items.Count, items);
            return collection;
        }

        public class TestQuery
        {
            public Guid? OptionalGuid { get; set; }

            public int AnInt { get; set; }

            public String FancyString { get; set; }

            public String PRETest { get; set; }
        }

        public class Test
        {
            public int IntVal { get; set; }

            public String StringVal { get; set; }
        }

        public class TestCollection : CollectionViewWithQuery<Test, TestQuery>
        {
            public TestCollection(TestQuery query, int total, IEnumerable<Test> items) : base(query, total, items)
            {
            }
        }
    }
}
