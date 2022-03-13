using Microsoft.Extensions.DependencyInjection;
using Moq;
using Threax.NJsonSchema.Generation;
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
            
        }

        protected virtual async Task CreateAsyncMocks()
        {
            mockup.Add<IValidSchemaTypeManager>(s =>
            {
                var mock = new Mock<IValidSchemaTypeManager>();
                mock.Setup(i => i.IsValid(It.IsAny<Type>())).Returns(true);
                return mock.Object;
            });

            mockup.Add<JsonSchemaGenerator>(s => new JsonSchemaGenerator(HalcyonConvention.DefaultJsonSchemaGeneratorSettings));

            mockup.Add<ISchemaBuilder>(s => new SchemaBuilder(s.Get<JsonSchemaGenerator>(), s.Get<IValidSchemaTypeManager>()));

            //This is setup outside the mock callbacks, so we can do async properly
            //The get must be after the registrations
            EndpointClientDefinition endpoint = null;

            mockup.TryAdd<IClientGenerator>(s =>
            {
                if(endpoint == null)
                {
                    throw new InvalidOperationException("This should not happen. EndpointClientDefinition not yet resolved and is null.");
                }

                var mock = new Mock<IClientGenerator>();
                IEnumerable<EndpointClientDefinition> mockEndpoints = new List<EndpointClientDefinition>() { endpoint };
                mock.Setup(i => i.GetEndpointDefinitions()).Returns(Task.FromResult(mockEndpoints));
                return mock.Object;
            });

            var schemaBuilder = mockup.Get<ISchemaBuilder>();
            endpoint = await CreateEndpointDefinition(schemaBuilder);
        }

        /// <summary>
        /// This is a generic endpoint definition generator, you can override it in your test class if you want to send something different.
        /// </summary>
        /// <param name="schemaBuilder"></param>
        /// <returns></returns>
        protected virtual async Task<EndpointClientDefinition> CreateEndpointDefinition(ISchemaBuilder schemaBuilder)
        {
            var endpoint = new EndpointClientDefinition(typeof(TResult), await schemaBuilder.GetSchema(typeof(TResult)));
            var endpointDoc = new EndpointDoc();
            endpointDoc.RequestSchema = await schemaBuilder.GetSchema(typeof(TInput));
            endpoint.AddLink(new EndpointClientLinkDefinition("Save", endpointDoc, false));
            return endpoint;
        }

        [Fact]
        protected async Task Typescript()
        {
            await CreateAsyncMocks();
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
            await CreateAsyncMocks();
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

        [Fact]
        protected async Task Php()
        {
            await CreateAsyncMocks();
            var clientWriter = new PhpClientWriter(mockup.Get<IClientGenerator>(), new PhpOptions()
            {
                Namespace = "phptest\\client"
            });

            using (var writer = new StreamWriter(new MemoryStream()))
            {
                await clientWriter.CreateClient(writer);
                writer.Flush();
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(writer.BaseStream))
                {
                    var code = reader.ReadToEnd();
                    TestCode($"{GetType().Name}.php", code);
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
