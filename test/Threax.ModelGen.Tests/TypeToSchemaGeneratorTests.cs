using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Threax.ModelGen.Tests
{
    public class TypeToSchemaGeneratorTests
    {
        [Fact]
        public async Task MissingRightProp()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => TypeToSchemaGenerator.CreateSchema(typeof(Left)));
        }

        [Fact]
        public async Task MissingLeftProp()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => TypeToSchemaGenerator.CreateSchema(typeof(Right2)));
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
