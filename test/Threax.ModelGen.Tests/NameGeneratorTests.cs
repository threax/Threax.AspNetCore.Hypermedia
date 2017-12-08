using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Threax.ModelGen.Tests
{
    public class NameGeneratorTests
    {
        [Theory]
        [InlineData("PascalName", "pascalName")]
        [InlineData("PascalName", "PascalName")]
        [InlineData("Event", "Event")]
        public void CreatePascal(String expected, String modelName)
        {
            Assert.Equal(expected, NameGenerator.CreatePascal(modelName));
        }

        [Theory]
        [InlineData("camelName", "camelName")]
        [InlineData("camelName", "CamelName")]
        [InlineData("@event", "Event")]
        public void CreateCamel(String expected, String modelName)
        {
            Assert.Equal(expected, NameGenerator.CreateCamel(modelName));
        }
    }
}
