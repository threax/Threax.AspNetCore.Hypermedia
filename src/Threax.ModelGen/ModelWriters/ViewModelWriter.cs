using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    class ViewModelWriter : ClassWriter
    {
        public ViewModelWriter(bool hasCreated, bool hasModified) : base(hasCreated, hasModified, new AttributeBuilder() { BuildRequired = false, BuildMaxLength = false })
        {
        }

        public override void AddUsings(StringBuilder sb, string ns)
        {
            base.AddUsings(sb, ns);
            sb.AppendLine(
$@"using {ns}.Models;
using {ns}.Controllers.Api;"
            );
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            sb.AppendLine( 
$@"    [HalModel]
    [HalSelfActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Get))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Update))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Delete))]
    public partial class {name} : I{name}, I{name}Id {GetAdditionalInterfaces()}
    {{"
            );

            CreateProperty(sb, $"{name}Id", new TypeWriterPropertyInfo<Guid>());
        }

        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            sb.AppendLine("        [UiOrder]");
            base.CreateProperty(sb, name, info);
        }
    }
}
