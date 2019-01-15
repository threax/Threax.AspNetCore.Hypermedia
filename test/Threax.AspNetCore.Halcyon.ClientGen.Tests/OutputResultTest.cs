using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class OutputResultTest : FileGenTests<OutputWithEmbeddedTest.Input, OutputWithEmbeddedTest.Output>
    {
        protected override async Task<EndpointClientDefinition> CreateEndpointDefinition(ISchemaBuilder schemaBuilder)
        {
            var endpoint = new EndpointClientDefinition(typeof(Output), await schemaBuilder.GetSchema(typeof(Output)));
            var endpointDoc = new EndpointDoc();
            endpointDoc.RequestSchema = await schemaBuilder.GetSchema(typeof(Input));
            endpointDoc.ResponseSchema = await schemaBuilder.GetSchema(typeof(Output));
            endpoint.AddLink(new EndpointClientLinkDefinition("Save", endpointDoc, false));
            return endpoint;
        }

        [HalModel]
        public class Input
        {

        }

        [HalModel]
        [HalActionLink(typeof(TestController), nameof(TestController.Save))]
        public class Output
        {
            
        }

        public class TestController : Controller
        {
            [HalRel("Save")]
            [HttpPost]
            public Output Save([FromForm] Input input)
            {
                return new Output();
            }
        }
    }
}
