using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    class InputModelWriter : ClassWriter
    {
        public InputModelWriter() : base(false, false, new AttributeBuilder())
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
$@"    [HalModel]
    public partial class {name}Input : I{name}
    {{"
            );
        }

        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            sb.AppendLine($@"        [UiOrder]");
            base.CreateProperty(sb, name, info);
        }
    }
}
