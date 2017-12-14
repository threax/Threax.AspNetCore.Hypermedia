using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tests;
using Threax.ModelGen.TestGenerators;
using Xunit;

namespace Threax.ModelGen.Tests.Models
{
    public class AbstractOnViewModelTests : ModelTests<AbstractOnViewModelTests.Value>
    {
        public class Value
        {
            [AbstractOnViewModel]
            public int Num { get; set; }
        }
    }
}
