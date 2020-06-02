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
    public class AbstractOnInputModelTests : ModelTests<AbstractOnInputModelTests.Value>
    {
        public class Value
        {
            [AbstractOnInputModel]
            public int Num { get; set; }
        }
    }
}
