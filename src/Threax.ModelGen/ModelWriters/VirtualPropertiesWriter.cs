using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class VirtualPropertiesWriter : AbstractTypeWriter
    {
        private IAttributeBuilder attributeBuilder;

        public VirtualPropertiesWriter(IAttributeBuilder attributeBuilder)
        {
            this.attributeBuilder = attributeBuilder;
        }

        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            if (info.IsVirtual)
            {
                attributeBuilder.BuildAttributes(sb, name, info, "        ");
                sb.AppendLine($"        public virtual {info.ClrType} {name} {{ get; set; }}");
                sb.AppendLine();
            }
        }
    }
}
