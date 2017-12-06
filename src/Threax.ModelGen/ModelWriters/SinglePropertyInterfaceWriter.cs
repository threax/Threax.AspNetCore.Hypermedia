using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class SinglePropertyInterfaceWriter : InterfaceWriter
    {
        public SinglePropertyInterfaceWriter(bool hasCreated, bool hasModified) : base(hasCreated, hasModified)
        {
        }

        public override void StartType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine(
$@"    public partial interface I{name}_{PropName}
    {{"
            );
        }

        public String PropName { get; set; }
    }
}
