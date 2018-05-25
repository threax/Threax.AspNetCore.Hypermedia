using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NJsonSchema.Generation;
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
        public InputWithEnumTest()
        {
            mockup.Add<IClientGenerator>(s =>
            {
                var schemaBuilder = s.Get<ISchemaBuilder>();

                var mock = new Mock<IClientGenerator>();

                IEnumerable<EndpointClientDefinition> mockEndpoints = new List<EndpointClientDefinition>()
                {
                    CreateEndpoint<InputWithEnumTest.Input, InputWithEnumTest.Output>(schemaBuilder),
                    CreateEndpoint<InputWithEnumTest.AnotherInput, InputWithEnumTest.Output>(schemaBuilder)
                };
                mock.Setup(i => i.GetEndpointDefinitions()).Returns(Task.FromResult(mockEndpoints));
                return mock.Object;
            });
        }

        private static EndpointClientDefinition CreateEndpoint<TInput, TResult>(ISchemaBuilder schemaBuilder)
        {
            var endpoint = new EndpointClientDefinition(typeof(TResult), schemaBuilder.GetSchema(typeof(TResult)).GetAwaiter().GetResult());
            var endpointDoc = new EndpointDoc();
            endpointDoc.RequestSchema = schemaBuilder.GetSchema(typeof(TInput)).GetAwaiter().GetResult();
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
