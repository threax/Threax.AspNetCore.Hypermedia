using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class PredefinedAttributeBuilder : AttributeBuilderChain
    {
        private String attributeString;

        public PredefinedAttributeBuilder(String attributeString, IAttributeBuilder next = null) : base(next)
        {
            this.attributeString = attributeString;
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            sb.Append(spaces);
            sb.AppendLine(attributeString);
            base.BuildAttributes(sb, name, prop, spaces);
        }
    }
}
