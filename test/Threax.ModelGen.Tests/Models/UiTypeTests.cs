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
    public class UiTypeTests : ModelTests<UiTypeTests.Value>
    {
        public class Value
        {
            [CheckboxUiType]
            public bool Checkbox { get; set; }

            [DateUiType]
            public DateTime DateOnly { get; set; }

            [HiddenUiType]
            public String Hidden { get; set; }

            [PasswordUiType]
            public String Password { get; set; }

            [SelectUiType]
            public String Select { get; set; }

            [TextAreaUiType]
            public String TextArea { get; set; }

            [UiType("custom")]
            public String CustomType { get; set; }
        }
    }
}
