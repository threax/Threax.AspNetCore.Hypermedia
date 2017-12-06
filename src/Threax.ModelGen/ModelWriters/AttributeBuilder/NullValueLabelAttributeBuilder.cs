using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class NullValueLabelAttributeBuilder : AttributeBuilderChain
    {
        public NullValueLabelAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            var nullValue = prop.NullValueLabel;
            if (!String.IsNullOrEmpty(nullValue))
            {
                sb.AppendLine(GetAttr(nullValue, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetAttr(String nullValue, String spaces)
        {
            return $@"{spaces}[NullValueLabel(""{nullValue}"")]";
        }
    }
}
