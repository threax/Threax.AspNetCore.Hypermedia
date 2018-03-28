using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class SchemaJsonConverterTests
    {
        private Mockup mockup = new Mockup().SetupGlobal();

        public SchemaJsonConverterTests()
        {
            
        }

        [Fact]
        public Task TestSimple()
        {
            return TestSchema(typeof(TestType), "TestSimple.json");
        }

        [Fact]
        public Task TestSimpleArray()
        {
            return TestSchema(typeof(TestSimpleArray), "TestSimpleArray.json");
        }

        [Fact]
        public Task TestComplexArray()
        {
            return TestSchema(typeof(TestComplexArrayType), "TestComplexArray.json");
        }

        private async Task TestSchema(Type type, String Filename)
        {
            var schemaBuilder = mockup.Get<ISchemaBuilder>();
            var schema = await schemaBuilder.GetSchema(type);
            var converter = mockup.Get<SchemaJsonConverter>();
            var serializer = JsonSerializer.Create(HalcyonConvention.DefaultJsonSerializerSettings);
            String finalJson;
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                converter.WriteJson(jsonWriter, schema, serializer);
                jsonWriter.Flush();
                streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(stream))
                {
                    finalJson = streamReader.ReadToEnd();
                }
            }

            FileUtils.WriteTestFile(this.GetType(), Filename, finalJson);
            var expected = FileUtils.ReadTestFile(this.GetType(), Filename);
            Assert.Equal(expected, finalJson);
        }
    }

    public class TestComplexArrayType
    {
        public List<TestType> Children { get; set; }
    }

    public class TestType
    {
        public String Name { get; set; }
    }

    public class TestSimpleArray
    {
        public List<Guid> Ids { get; set; }
    }
}
