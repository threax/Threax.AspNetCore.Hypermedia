using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class CreateInputModel : ITypeWriter
    {
        protected String args;

        public CreateInputModel(String args)
        {
            this.args = args;
        }

        public virtual void StartType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine(
$@"        public static {name}Input CreateInput(String seed = """"{args})
        {{
            return new {name}Input()
            {{"
            );
        }

        public void EndType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine(
$@"            }};
        }}"
            );
        }

        public void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            switch (info.ClrType.ToLowerInvariant())
            {
                case "string":
                    sb.AppendLine($"                {name} = {name} != null ? {name} : $\"{name} {{seed}}\",");
                    break;
                default:
                    sb.AppendLine($"                {name} = {name},");
                    break;
            }
        }

        public void AddUsings(StringBuilder sb, string ns)
        {
            
        }

        public void EndNamespace(StringBuilder sb)
        {
            
        }

        public void StartNamespace(StringBuilder sb, string name)
        {
            
        }
    }
}
