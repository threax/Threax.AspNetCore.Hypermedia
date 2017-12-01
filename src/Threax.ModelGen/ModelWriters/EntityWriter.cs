using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    class EntityWriter : ClassWriter
    {
        public EntityWriter(bool hasCreated, bool hasModified)
            :base(hasCreated, hasModified, new AttributeBuilder() { BuildDisplay = false, BuildRequired = false })
        {
            
        }

        public override void AddUsings(StringBuilder sb, string ns)
        {
            base.AddUsings(sb, ns);
            sb.AppendLine($"using {ns}.Models;");
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            sb.AppendLine(
$@"    public partial class {name}Entity : I{name}, I{name}Id{AdditionalInterfacesText} {GetAdditionalInterfaces()}
    {{
        [Key]"
            );

            CreateProperty(sb, $"{name}Id", new TypeWriterPropertyInfo<Guid>());
        }

        public String AdditionalInterfaces { get; set; }

        private String AdditionalInterfacesText
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AdditionalInterfaces))
                {
                    return "";
                }
                else
                {
                    return ", " + AdditionalInterfaces;
                }
            }
        }
    }
}
