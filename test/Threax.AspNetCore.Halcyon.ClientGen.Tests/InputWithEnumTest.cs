using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Threax.NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.ClientGen.Tests
{
    public enum TestEnum
    {
        [Display(Name = "Value One")]
        One = 0,
        [Display(Name = "Value Two")]
        Two = 1
    }

    public class InputWithEnumTest : FileGenTests<InputWithEnumTest.Input, InputWithEnumTest.Output>
    {
        protected override async Task CreateAsyncMocks()
        {
            await base.CreateAsyncMocks();

            var schemaBuilder = mockup.Get<ISchemaBuilder>();

            IEnumerable<EndpointClientDefinition> mockEndpoints = new List<EndpointClientDefinition>()
            {
                await CreateEndpoint<InputWithEnumTest.Input, InputWithEnumTest.Output>(schemaBuilder),
                await CreateEndpoint<InputWithEnumTest.AnotherInput, InputWithEnumTest.Output>(schemaBuilder)
            };

            mockup.Add<IClientGenerator>(s =>
            {
                var mock = new Mock<IClientGenerator>();
                mock.Setup(i => i.GetEndpointDefinitions()).Returns(Task.FromResult(mockEndpoints));
                return mock.Object;
            });
        }

        private static async Task<EndpointClientDefinition> CreateEndpoint<TInput, TResult>(ISchemaBuilder schemaBuilder)
        {
            var endpoint = new EndpointClientDefinition(typeof(TResult), await schemaBuilder.GetSchema(typeof(TResult)));
            var endpointDoc = new EndpointDoc();
            endpointDoc.RequestSchema = await schemaBuilder.GetSchema(typeof(TInput));
            endpoint.AddLink(new EndpointClientLinkDefinition("Save", endpointDoc, false));
            return endpoint;
        }

        [HalModel]
        public class Input
        {
            public TestEnum EnumValue { get; set; }
        }

        [HalModel]
        public class AnotherInput
        {
            public TestEnum EnumValue { get; set; }
        }

        [HalModel]
        [HalActionLink(typeof(TestController), nameof(TestController.Save))]
        public class Output
        {
            public TestEnum EnumValue { get; set; }
        }

        public class TestController : Controller
        {
            [HalRel("Save")]
            [HttpPost]
            public void Save([FromForm] Input input)
            {

            }

            [HalRel("Save")]
            [HttpPost]
            public void Update([FromForm] AnotherInput input)
            {

            }
        }
    }
}
