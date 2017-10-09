using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class ViewModelWriter : ClassWriter
    {
        public override string AddUsings(string ns)
        {
            return $@"{base.AddUsings(ns)}
using {ns}.Models;
using {ns}.Controllers.Api;";
        }

        public override String StartType(String name, String pluralName)
        {
            return 
$@"    [HalModel]
    [HalSelfActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Get))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Update))]
    [HalActionLink(typeof({pluralName}Controller), nameof({pluralName}Controller.Delete))]
    public class {name} : I{name}, I{name}Id{AdditionalInterfacesText}
    {{
{CreateProperty("Guid", $"{name}Id")}";
        }

        public String AdditionalInterfaces { get; set; }

        private String AdditionalInterfacesText
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AdditionalInterfaces))
                {
                    return "";
                }
                else
                {
                    return ", " + AdditionalInterfaces;
                }
            }
        }

        public override string AddMaxLength(int length, string errorMessage)
        {
            return "";
        }

        public override string AddRequired(string errorMessage)
        {
            return "";
        }
    }
}
