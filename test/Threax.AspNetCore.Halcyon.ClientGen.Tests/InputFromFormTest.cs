using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Threax.NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.ClientGen.Tests
{
    public class InputFromFormTest : FileGenTests<InputFromFormTest.Input, InputFromFormTest.Output>
    {
        [HalModel]
        public class Input
        {
            public String Name { get; set; }
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
            public void Save([FromForm] Input input)
            {

            }
        }
    }
}
