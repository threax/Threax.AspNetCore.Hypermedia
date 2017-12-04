using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public class ClassWriter : ITypeWriter
    {
        private bool hasCreated = false;
        private bool hasModified = false;
        protected IAttributeBuilder attributeBuilder { get; private set; }

        public ClassWriter(bool hasCreated, bool hasModified, IAttributeBuilder attributeBuilder)
        {
            this.hasCreated = hasCreated;
            this.hasModified = hasModified;
            this.attributeBuilder = attributeBuilder;
        }

        public virtual void AddUsings(StringBuilder sb, String ns)
        {
            if (WriteUsings)
            {
                sb.AppendLine(
    @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.UIAttrs;"
                );

                if (hasCreated || hasModified)
                {
                    sb.AppendLine("using Threax.AspNetCore.Tracking;");
                }

                if (!String.IsNullOrEmpty(AdditionalUsings))
                {
                    sb.AppendLine(AdditionalUsings);
                }
            }
        }

        public String AdditionalUsings { get; set; }

        public bool WriteUsings { get; set; } = true;

        public virtual void StartType(StringBuilder sb, String name, String pluralName)
        {
            if (WriteType)
            {
                sb.AppendLine(
    $@"    public class {name} {GetAdditionalInterfaces()}
    {{"
                );
            }
        }

        public virtual void EndType(StringBuilder sb, String name, String pluralName)
        {
            if (WriteType)
            {
                if (hasCreated)
                {
                    CreateProperty(sb, "Created", new TypeWriterPropertyInfo<DateTime>());
                }

                if (hasModified)
                {
                    CreateProperty(sb, "Modified", new TypeWriterPropertyInfo<DateTime>());
                }

                sb.AppendLine("    }");
            }
        }

        public bool WriteType { get; set; } = true;

        public virtual void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            if (WriteProperties)
            {
                String @virtual = "";
                if (WritePropertiesVirtual)
                {
                    @virtual = "virtual ";
                }
                attributeBuilder.BuildAttributes(sb, name, info, "        ");
                sb.AppendLine($"        public {@virtual}{info.ClrType} {name} {{ get; set; }}");
                sb.AppendLine();
            }
        }

        public bool WriteProperties { get; set; } = true;

        public bool WritePropertiesVirtual { get; set; } = false;

        public virtual void EndNamespace(StringBuilder sb)
        {
            if (WriteNamespace)
            {
                sb.AppendLine("}");
            }
        }

        public virtual void StartNamespace(StringBuilder sb, string name)
        {
            if (WriteNamespace)
            {
                sb.AppendLine();
                sb.AppendLine(
$@"namespace {name} 
{{"
                );
            }
        }

        public bool WriteNamespace { get; set; } = true;

        protected String GetAdditionalInterfaces()
        {
            String extraInterfaces = "";
            if (hasCreated && hasModified)
            {
                extraInterfaces += ", ICreatedModified";
            }
            else
            {
                if (hasCreated)
                {
                    extraInterfaces += ", ICreated";
                }

                if (hasModified)
                {
                    extraInterfaces += ", IModified";
                }
            }
            return extraInterfaces;
        }
    }
}
