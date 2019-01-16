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
    public class OutputWithCollectionTest : FileGenTests<OutputWithCollectionTest.Input, OutputWithCollectionTest.OutputCollection>
    {
        /**
         * Note that this test will not create an EmbeddedObjectResult in its output since the provider is faked.
         * When running for real that class will be discovered and added in a separate step.
         */

        [HalModel]
        public class Input : PagedCollectionQuery
        {

        }

        [HalModel]
        public class OutputCollection : PagedCollectionViewWithQuery<Output, Input>
        {
            public OutputCollection(Input query, int total, IEnumerable<Output> items) : base(query, total, items)
            {
            }
        }

        [HalModel]
        [HalActionLink(typeof(TestController), nameof(TestController.Save))]
        public class Output
        {
            
        }

        [HalModel]
        [HalActionLink(typeof(TestController), nameof(TestController.EmbeddedAction))]
        public class EmbeddedObject
        {
            public int Value { get; set; }
        }

        public class TestController : Controller
        {
            [HalRel("Save")]
            [HttpPost]
            public void Save([FromForm] Input input)
            {

            }

            [HalRel("EmbeddedAction")]
            [HttpPost]
            public void EmbeddedAction()
            {

            }
        }
    }
}
