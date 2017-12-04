using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class MainModelWriter : ClassWriter
    {
        public class Args
        {
            public String Name { get; set; }

            public String BaseClassName { get; set; }

            public String ModelSuffix { get; set; }
        }

        private String baseClass;
        private String modelSuffix;
        private Action<StringBuilder, Args> writeClass;

        public MainModelWriter(String baseClass, String modelSuffix, IAttributeBuilder propAttrBuilder, IAttributeBuilder classAttrBuilder, bool hasCreated, bool hasModified, Action<StringBuilder, Args> writeClass) : base(hasCreated, hasModified, propAttrBuilder, classAttrBuilder)
        {
            this.baseClass = baseClass;
            this.modelSuffix = modelSuffix;
            this.writeClass = writeClass;
        }

        public override sealed void StartType(StringBuilder sb, String name, String pluralName)
        {
            var baseClassName = "";
            if (baseClass != null)
            {
                baseClassName = $"{name}{modelSuffix}Base, ";
                sb.AppendLine(baseClass);
            }

            ClassAttrBuilder?.BuildAttributes(sb, name, new NoWriterInfo(), "    ");
            writeClass(sb, new Args()
            {
                BaseClassName = baseClassName,
                Name = name,
                ModelSuffix = modelSuffix
            });
        }

    //    private void WriteClassFallback(StringBuilder sb, string name, string baseClassName)
    //    {
    //        sb.AppendLine(
    //        $@"    public partial class {name}{modelSuffix} : {baseClassName}I{name}
    //{{"
    //                    );
    //    }
    }
}
