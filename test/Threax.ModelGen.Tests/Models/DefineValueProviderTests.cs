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
    public class FakeValueProvider //This would normally extend IValueProvider, but we are just testing that the type goes through.
    {

    }

    public class DefineValueProviderTests : ModelTests<DefineValueProviderTests.Value>
    {
        public class Value
        {
            [DefineValueProvider(typeof(FakeValueProvider))]
            [Queryable]
            public String Things { get; set; }
        }
    }
}
