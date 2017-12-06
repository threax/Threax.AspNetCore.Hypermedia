using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class UiSearchAttributeBuilder : AttributeBuilderChain
    {
        public UiSearchAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.ShowOnQueryUi)
            {
                sb.AppendLine(Get(prop.Order.Value, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String Get(int order, String spaces)
        {
            return $@"{spaces}[UiSearch]";
        }
    }
}
