﻿using NJsonSchema;
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
    //This does not work
    public class DefineNamespaceTests : ModelTests<DefineNamespaceTests.Value>
    {
        [AddNamespaces(@"using An.Extra.Namespace;
using Can.Be.Multiline;")]
        public class Value
        {
            
        }
    }
}