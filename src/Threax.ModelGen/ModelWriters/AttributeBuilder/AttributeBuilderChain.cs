using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public abstract class AttributeBuilderChain : IAttributeBuilder
    {
        private IAttributeBuilder next;

        public AttributeBuilderChain(IAttributeBuilder next)
        {
            this.next = next;
        }

        public virtual void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            next?.BuildAttributes(sb, name, prop, spaces);
        }
    }
}
