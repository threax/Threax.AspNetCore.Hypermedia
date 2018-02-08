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
    public class CamelCaseConversionTests : ModelTests<CamelCaseConversionTests.Value>
    {
        public class Value
        {
            public int NormalName { get; set; }

            public int CAPSName { get; set; }

            public int ALLCAPS { get; set; }
        }
    }
}
