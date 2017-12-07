using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class BaseModelWriter : ClassWriter
    {
        protected String classSuffix;

        public BaseModelWriter(String classSuffix, IAttributeBuilder propAttributeBuilder, IAttributeBuilder classAttrBuilder = null) : base(false, false, propAttributeBuilder, classAttrBuilder)
        {
            this.classSuffix = classSuffix;
            WriteUsings = false;
            WriteNamespace = false;
            WriteAsAbstractClass = true;
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            ClassAttrBuilder?.BuildAttributes(sb, name, new NoWriterInfo(), "    ");

            sb.AppendLine(
$@"    public {GetInheritance()} class {CreateBaseClassName(name, classSuffix)}{InterfaceListBuilder.Build(InheritFrom)}
    {{"
            );
        }

        public static String CreateBaseClassName(String name, String classSuffix)
        {
            return $"{name}{classSuffix}Base";
        }

        public IEnumerable<String> InheritFrom { get; set; } = new String[0];
    }
}
