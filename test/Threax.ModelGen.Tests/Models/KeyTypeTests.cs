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
    public class KeyTypeGuidTests : ModelTests<KeyTypeGuidTests.Value>
    {
        public class Value
        {

        }
    }

    public class KeyTypeIntTests : ModelTests<KeyTypeIntTests.Value>
    {
        [KeyType(typeof(int))]
        public class Value
        {

        }
    }

    public class KeyTypeStringTests : ModelTests<KeyTypeStringTests.Value>
    {
        [KeyType(typeof(String))]
        public class Value
        {

        }
    }

    public class KeyTypeEnumTests : ModelTests<KeyTypeEnumTests.Value>
    {
        public enum KeyEnum
        {
            Key1,
            Key2
        }

        [KeyType(typeof(KeyEnum))]
        public class Value
        {

        }
    }
}
