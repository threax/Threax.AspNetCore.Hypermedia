using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class NoAttributeBuilder : IAttributeBuilder
    {
        public void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {

        }
    }
}
