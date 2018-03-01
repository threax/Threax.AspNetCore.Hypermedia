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

    public class InputWithFileTest : FileGenTests
    {
        public InputWithFileTest()
        {
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
    }
}
