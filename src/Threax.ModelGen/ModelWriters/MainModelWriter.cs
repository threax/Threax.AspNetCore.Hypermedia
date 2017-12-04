using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class MainModelWriter : ClassWriter
    {
        private String baseClass;
        private String modelSuffix;
        private IAttributeBuilder classAttributes;

        public MainModelWriter(String baseClass, String modelSuffix, IAttributeBuilder propertyAttributes, IAttributeBuilder classAttributes, bool hasCreated, bool hasModified) : base(hasCreated, hasModified, propertyAttributes)
        {
            this.baseClass = baseClass;
            this.modelSuffix = modelSuffix;
            this.classAttributes = classAttributes;
        }

        public override void StartType(StringBuilder sb, String name, String pluralName)
        {
            var baseClassName = "";
            if (baseClass != null)
            {
                baseClassName = $"{name}{modelSuffix}Base, ";
                sb.AppendLine(baseClass);
            }

            classAttributes.BuildAttributes(sb, name, new NoWriterInfo(), "    ");

            sb.AppendLine(
$@"    public partial class {name}{modelSuffix} : {baseClassName}I{name}
    {{"
            );
        }
    }
}
