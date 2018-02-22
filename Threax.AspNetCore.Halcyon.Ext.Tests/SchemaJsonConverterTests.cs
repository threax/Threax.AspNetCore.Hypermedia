using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using Threax.AspNetCore.Halcyon.ClientGen.Tests;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class SchemaJsonConverterTests
    {
        private bool writeTests = false;

        private Mockup mockup = new Mockup();

        public SchemaJsonConverterTests()
        {
            mockup.Add<IValidSchemaTypeManager>(s =>
            {
                var mock = new Mock<IValidSchemaTypeManager>();
                mock.Setup(i => i.IsValid(It.IsAny<Type>())).Returns(true);
                return mock.Object;
            });

            mockup.Add<JsonSchemaGenerator>(s => new JsonSchemaGenerator(new JsonSchemaGeneratorSettings()
            {
                DefaultEnumHandling = EnumHandling.String,
                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase,
                FlattenInheritanceHierarchy = true,
            }));

            mockup.Add<ISchemaBuilder>(s => new SchemaBuilder(s.Get<JsonSchemaGenerator>(), s.Get<IValidSchemaTypeManager>()));

            mockup.Add<SchemaJsonConverter>(m => new SchemaJsonConverter());
        }

        [Fact]
        public void TestSimple()
        {
            TestSchema(typeof(TestType), "TestSimple.json");
        }

        [Fact]
        public void TestSimpleArray()
        {
            TestSchema(typeof(TestSimpleArray), "TestSimpleArray.json");
        }

        [Fact]
        public void TestComplexArray()
        {
            TestSchema(typeof(TestComplexArrayType), "TestComplexArray.json");
        }

        private void TestSchema(Type type, String Filename)
        {
            var schemaBuilder = mockup.Get<ISchemaBuilder>();
            var schema = schemaBuilder.GetSchema(type);
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

            if (writeTests)
            {
                FileUtils.WriteTestFile(this.GetType(), Filename, finalJson);
            }

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
