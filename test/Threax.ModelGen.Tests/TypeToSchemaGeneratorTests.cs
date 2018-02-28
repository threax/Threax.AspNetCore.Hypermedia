using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Threax.ModelGen.Tests
{
    public class TypeToSchemaGeneratorTests
    {
        [Fact]
        public void MissingRightProp()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => TypeToSchemaGenerator.CreateSchema(typeof(Left)));
        }

        [Fact]
        public void MissingLeftProp()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => TypeToSchemaGenerator.CreateSchema(typeof(Right2)));
        }

        public class Left
        {
            public List<Right> Rights { get; set; }
        }

        public class Right
        {

        }

        public class Left2
        {
            
        }

        public class Right2
        {
            public List<Left> Lefts { get; set; }
        }
    }
}
