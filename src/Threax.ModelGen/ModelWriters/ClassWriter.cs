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
        protected IAttributeBuilder AttributeBuilder { get; private set; }

        public ClassWriter(bool hasCreated, bool hasModified, IAttributeBuilder attributeBuilder)
        {
            this.hasCreated = hasCreated;
            this.hasModified = hasModified;
            this.AttributeBuilder = attributeBuilder;
        }

        public virtual void AddUsings(StringBuilder sb, String ns)
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

            if(hasCreated || hasModified)
            {
                sb.AppendLine("using Threax.AspNetCore.Tracking;");
            }
        }

        public virtual void StartType(StringBuilder sb, String name, String pluralName)
        {
            sb.AppendLine(
$@"    public class {name} {GetAdditionalInterfaces()}
    {{"
            );
        }

        public virtual void EndType(StringBuilder sb, String name, String pluralName)
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

        public virtual void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            AttributeBuilder.BuildAttributes(sb, name, info, "        ");
            sb.AppendLine($"        public {info.ClrType} {name} {{ get; set; }}");
            sb.AppendLine();
        }

        public virtual void EndNamespace(StringBuilder sb)
        {
            sb.AppendLine("}");
        }

        public virtual void StartNamespace(StringBuilder sb, string name)
        {
            sb.AppendLine(
$@"namespace {name} 
{{"
            );
        }

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
