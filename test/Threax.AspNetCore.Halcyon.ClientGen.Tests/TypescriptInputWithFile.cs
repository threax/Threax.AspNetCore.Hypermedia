using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.ClientGen.Tests
{
    [HalModel]
    public class InputWithFile
    {
        public IFormFile File { get; set; }
    }

    [HalModel]
    [HalActionLink(typeof(FileController), nameof(FileController.Save))]
    public class FileResult
    {

    }

    public class FileController : Controller
    {
        [HalRel("Save")]
        [HttpPost]
        public void Save([FromForm] InputWithFile input)
        {

        }
    }

    public class TypescriptInputWithFile
    {
        private Mockup mockup = new Mockup();
        private bool WriteTestFiles = false;

        public TypescriptInputWithFile()
        {
            mockup.Add<IValidSchemaTypeManager>(s =>
            {
                var mock = new Mock<IValidSchemaTypeManager>();
                mock.Setup(i => i.IsValid(It.IsAny<Type>())).Returns(true);
                return mock.Object;
            });

            mockup.Add<JsonSchemaGenerator>(s => new JsonSchemaGenerator(new JsonSchemaGeneratorSettings()));

            mockup.Add<ISchemaBuilder>(s => new SchemaBuilder(s.Get<JsonSchemaGenerator>(), s.Get<IValidSchemaTypeManager>()));

            mockup.Add<IClientGenerator>(s =>
            {
                var schemaBuilder = s.Get<ISchemaBuilder>();

                var mock = new Mock<IClientGenerator>();
                var endpoint = new EndpointClientDefinition(typeof(FileResult), schemaBuilder.GetSchema(typeof(FileResult)));
                var endpointDoc = new EndpointDoc();
                endpointDoc.RequestSchema = schemaBuilder.GetSchema(typeof(InputWithFile));
                endpoint.AddLink(new EndpointClientLinkDefinition("Save", endpointDoc, false));
                var mockEndpoints = new List<EndpointClientDefinition>() { endpoint };
                mock.Setup(i => i.GetEndpointDefinitions()).Returns(mockEndpoints);
                return mock.Object;
            });
        }

        [Fact]
        public void Test()
        {
            var typescriptWriter = new TypescriptClientWriter(mockup.Get<IClientGenerator>());
            using(var writer = new StreamWriter(new MemoryStream()))
            {
                typescriptWriter.CreateClient(writer);
                writer.Flush();
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(writer.BaseStream))
                {
                    var code = reader.ReadToEnd();
                    TestCode($"TypescriptInputWithFile.ts", code);
                }
            }
        }

        private void TestCode(String fileName, String code)
        {
            if (WriteTestFiles)
            {
                FileUtils.WriteTestFile(this.GetType(), fileName, code);
            }

            //This works around line ending goofyness.
            Assert.Equal(FileUtils.ReadTestFile(this.GetType(), fileName).Replace("\r\n", "\n"), code.Replace("\r\n", "\n"));
        }
    }
}
