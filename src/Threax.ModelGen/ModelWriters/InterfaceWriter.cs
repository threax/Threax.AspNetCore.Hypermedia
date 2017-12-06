using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public class InterfaceWriter : ClassWriter
    {
        public InterfaceWriter(bool hasCreated, bool hasModified) : base(hasCreated, hasModified, new NoAttributeBuilder())
        {
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            sb.AppendLine(
$@"    public partial interface I{name} 
    {{"
            );
        }

        public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            sb.AppendLine($"        {info.ClrType} {name} {{ get; set; }}");
            sb.AppendLine();
        }

        public override void EndNamespace(StringBuilder sb)
        {
            if(WriteEndNamespace)
            {
                base.EndNamespace(sb);
            }
        }

        public bool WriteEndNamespace { get; set; } = true;
    }
}
