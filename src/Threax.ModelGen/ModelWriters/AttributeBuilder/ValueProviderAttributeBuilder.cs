using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class ValueProviderAttributeBuilder : AttributeBuilderChain
    {
        public ValueProviderAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            var valueProviderType = prop.ValueProviderType;
            if (!String.IsNullOrEmpty(valueProviderType))
            {
                sb.AppendLine(GetAttr(valueProviderType, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetAttr(String valueProviderType, String spaces)
        {
            return $@"{spaces}[ValueProvider(typeof({valueProviderType}))]";
        }
    }
}
