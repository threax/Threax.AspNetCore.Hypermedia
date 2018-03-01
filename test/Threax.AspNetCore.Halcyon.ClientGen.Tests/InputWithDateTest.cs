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
    public class InputWithDateTest : FileGenTests<InputWithDateTest.Input, InputWithDateTest.Output>
    {
        [HalModel]
        public class Input
        {
            public DateTime Date { get; set; }
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
