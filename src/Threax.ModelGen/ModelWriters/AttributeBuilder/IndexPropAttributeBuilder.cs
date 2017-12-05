using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class IndexPropAttributeBuilder : AttributeBuilderChain
    {
        public IndexPropAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.HasIndexProp)
            {
                sb.AppendLine(GetAttribute(prop.DisplayName, spaces, prop.IndexPropClustered, prop.IndexPropUnique));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetAttribute(String displayName, String spaces, bool clustered, bool unique)
        {
            String clusteredStr = "";
            String uniqueStr = "";
            String startConstructor = "";
            String endConstructor = "";
            if (clustered || unique)
            {
                startConstructor = "(";
                endConstructor = ")";
            }

            if (clustered)
            {
                clusteredStr = $"Clustered = {clustered}";
            }

            if (unique)
            {
                uniqueStr = $"Unique = {unique}";
                if (clustered)
                {
                    uniqueStr = $", {uniqueStr}";
                }
            }

            return $@"{spaces}[IndexProp{startConstructor}{clusteredStr}{uniqueStr}{endConstructor}]";
        }
    }
}
