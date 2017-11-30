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

        public virtual string StartType(string name, string pluralName)
        {
            return 
$@"        public static {name}Input CreateInput(String seed = """"{args})
        {{
            return new {name}Input()
            {{";
        }

        public string EndType(string name, string pluralName)
        {
            return 
$@"            }};
        }}";
        }

        public string CreateProperty(string name, IWriterPropertyInfo info)
        {
            switch (info.ClrType.ToLowerInvariant())
            {
                case "string":
                    return $"                {name} = {name} != null ? {name} : $\"{name} {{seed}}\",";
                default:
                    return $"                {name} = {name},";
            }
        }

        public string AddDisplay(string name)
        {
            return "";
        }

        public string AddMaxLength(int length, string errorMessage)
        {
            return "";
        }

        public string AddRequired(string errorMessage)
        {
            return "";
        }

        public string AddTypeDisplay(string name)
        {
            return "";
        }

        public string AddUsings(string ns)
        {
            return "";
        }

        public string EndNamespace()
        {
            return "";
        }

        public string StartNamespace(string name)
        {
            return "";
        }
    }
}
