using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class UiOrderAttributeBuilder : AttributeBuilderChain
    {
        public UiOrderAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.Order != null)
            {
                sb.AppendLine(GetOrder(prop.Order.Value, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetOrder(int order, String spaces)
        {
            return $@"{spaces}[UiOrder(0, {order})]";
        }
    }
}
