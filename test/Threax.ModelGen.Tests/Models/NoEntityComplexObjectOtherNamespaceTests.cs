using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tests;
using Threax.ModelGen.TestGenerators;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Threax.ModelGen.Tests.Models
{
    /// <summary>
    /// Ensures that no entity does not generate a model relationship.
    /// </summary>
    public class NoEntityComplexObjectOtherNamespaceTests : ModelTests<NoEntityComplexObjectOtherNamespaceTests.Value>
    {
        //This can also apply to a whole class, but this is not tested here

        [AddNamespaces("using Threax.ModelGen.Tests.Models.OtherNamespace;")]
        public class Value
        { 
            [NoEntity]
            public OtherNamespace.OtherNamespaceClass Info { get; set; }
        }
    }
}

namespace Threax.ModelGen.Tests.Models.OtherNamespace
{
    public class OtherNamespaceClass
    {
        public String Prop1 { get; set; }

        public String Prop2 { get; set; }
    }
}