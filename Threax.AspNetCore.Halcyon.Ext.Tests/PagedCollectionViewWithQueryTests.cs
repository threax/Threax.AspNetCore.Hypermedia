using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class PagedCollectionViewWithQueryTests
    {
        private Mockup mockup = new Mockup().SetupGlobal();

        [Fact]
        public void Query()
        {

        }

        [Fact]
        public void Json()
        {
            var query = new TestQuery();
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

        public class TestQuery : PagedCollectionQuery
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

        public class TestCollection : PagedCollectionViewWithQuery<Test, TestQuery>
        {
            public TestCollection(TestQuery query, int total, IEnumerable<Test> items) : base(query, total, items)
            {
            }
        }
    }
}
