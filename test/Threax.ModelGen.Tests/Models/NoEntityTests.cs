﻿using Threax.NJsonSchema;
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
    public class NoEntityTests : ModelTests<NoEntityTests.Value>
    {
        //This can also apply to a whole class, but this is not tested here

        public class Value
        {
            [NoEntity]
            public String Info { get; set; }
        }
    }
}
