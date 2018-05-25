using Microsoft.Extensions.DependencyInjection;
using Moq;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.ClientGen.Tests
{
    public abstract class FileGenTests<TInput, TResult>
    {
        private bool WriteTestFiles = false;

        protected Mockup mockup = new Mockup();

        public FileGenTests()
        {
            mockup.Add<IValidSchemaTypeManager>(s =>
            {
                var mock = new Mock<IValidSchemaTypeManager>();
                mock.Setup(i => i.IsValid(It.IsAny<Type>())).Returns(true);
                return mock.Object;
            });

            mockup.Add<JsonSchemaGenerator>(s => new JsonSchemaGenerator(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));

            mockup.Add<ISchemaBuilder>(s => new SchemaBuilder(s.Get<JsonSchemaGenerator>(), s.Get<IValidSchemaTypeManager>()));

            mockup.Add<IClientGenerator>(s =>
            {
                var schemaBuilder = s.Get<ISchemaBuilder>();

                var mock = new Mock<IClientGenerator>();
                var endpoint = new EndpointClientDefinition(typeof(TResult), schemaBuilder.GetSchema(typeof(TResult)).GetAwaiter().GetResult());
                var endpointDoc = new EndpointDoc();
                endpointDoc.RequestSchema = schemaBuilder.GetSchema(typeof(TInput)).GetAwaiter().GetResult();
                endpoint.AddLink(new EndpointClientLinkDefinition("Save", endpointDoc, false));
                IEnumerable<EndpointClientDefinition> mockEndpoints = new List<EndpointClientDefinition>() { endpoint };
                mock.Setup(i => i.GetEndpointDefinitions()).Returns(Task.FromResult(mockEndpoints));
                return mock.Object;
            });
        }

        [Fact]
        protected async Task Typescript()
        {
            var clientWriter = new TypescriptClientWriter(mockup.Get<IClientGenerator>());
            using (var writer = new StreamWriter(new MemoryStream()))
            {
                await clientWriter.CreateClient(writer);
                writer.Flush();
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(writer.BaseStream))
                {
                    var code = reader.ReadToEnd();
                    TestCode($"{GetType().Name}.ts", code);
                }
            }
        }

        [Fact]
        protected async Task CSharp()
        {
            var clientWriter = new CSharpClientWriter(mockup.Get<IClientGenerator>(), new CSharpOptions()
            {
                Namespace = "Test"
            });

            using (var writer = new StreamWriter(new MemoryStream()))
            {
                await clientWriter.CreateClient(writer);
                writer.Flush();
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(writer.BaseStream))
                {
                    var code = reader.ReadToEnd();
                    TestCode($"{GetType().Name}.cs", code);
                }
            }
        }

        private void TestCode(String fileName, String code)
        {
            code = code.Replace("\r\n", "\n");

            if (WriteTestFiles)
            {
                FileUtils.WriteTestFile(this.GetType(), fileName, code);
            }

            Assert.Equal(FileUtils.ReadTestFile(this.GetType(), fileName).Replace("\r\n", "\n"), code);
        }
    }
}
