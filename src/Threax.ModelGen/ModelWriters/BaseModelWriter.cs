using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class BaseModelWriter : ClassWriter
    {
        private String classSuffix;

        public BaseModelWriter(String classSuffix, IAttributeBuilder propAttributeBuilder, IAttributeBuilder classAttrBuilder = null) : base(false, false, propAttributeBuilder, classAttrBuilder)
        {
            this.classSuffix = classSuffix;
            WriteUsings = false;
            WriteNamespace = false;
            WritePropertiesVirtual = true;
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            ClassAttrBuilder?.BuildAttributes(sb, name, new NoWriterInfo(), "    ");

            sb.AppendLine(
$@"    public partial class {name}{classSuffix}Base
    {{"
            );
        }
    }
}
