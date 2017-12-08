using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Threax.ModelGen.Tests
{
    public class TypeExtensionTests
    {
        enum TestEnum
        {
            Value1,
            Value2
        }

        class TestClass
        {

        }

        [Theory]
        [InlineData(typeof(TestEnum?), "TestEnum?")]
        [InlineData(typeof(TestEnum), "TestEnum")]
        [InlineData(typeof(TestClass), "TestClass")]
        [InlineData(typeof(String), "String")]
        public void GetSchemaFormat(Type type, String expected)
        {
            Assert.Equal(expected, type.GetSchemaFormat());
        }
    }
}
