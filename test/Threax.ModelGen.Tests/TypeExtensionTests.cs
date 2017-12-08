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

        class GenericTestClass<T1>
        {

        }

        class GenericTestClass<T1, T2>
        {

        }

        [Theory]
        [InlineData("TestEnum?", typeof(TestEnum?))]
        [InlineData("TestEnum", typeof(TestEnum))]
        [InlineData("TestClass", typeof(TestClass))]
        [InlineData("String", typeof(String))]
        [InlineData("GenericTestClass<Int32>", typeof(GenericTestClass<int>))]
        [InlineData("GenericTestClass<Int32, String>", typeof(GenericTestClass<int, String>))]
        [InlineData("GenericTestClass<TestClass, TestEnum?>", typeof(GenericTestClass<TestClass, TestEnum?>))]
        public void GetSchemaFormat(String expected, Type type)
        {
            Assert.Equal(expected, type.GetSchemaFormat());
        }
    }
}
