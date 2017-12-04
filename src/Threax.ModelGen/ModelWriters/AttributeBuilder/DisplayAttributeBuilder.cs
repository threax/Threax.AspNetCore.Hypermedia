using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class DisplayAttributeBuilder : AttributeBuilderChain
    {
        public DisplayAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (!String.IsNullOrEmpty(prop.DisplayName))
            {
                sb.AppendLine(GetDisplay(prop.DisplayName, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetDisplay(String displayName, String spaces)
        {
            return $@"{spaces}[Display(Name = ""{displayName}"")]";
        }
    }
}
