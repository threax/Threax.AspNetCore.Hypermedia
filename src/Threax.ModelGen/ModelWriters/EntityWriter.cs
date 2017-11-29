using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class EntityWriter : ClassWriter
    {
        public EntityWriter(bool hasCreated, bool hasModified)
            :base(hasCreated, hasModified)
        {
            
        }

        public override string AddUsings(string ns)
        {
            return $@"{base.AddUsings(ns)}
using {ns}.Models;";
        }

        public override String StartType(String name, String pluralName)
        {
            return $@"    public class {name}Entity : I{name}, I{name}Id{AdditionalInterfacesText} {GetAdditionalInterfaces()}
    {{
        [Key]
{CreateProperty("Guid", $"{name}Id")}";
        }

        public override string AddTypeDisplay(string name)
        {
            return "";
        }

        public override string AddDisplay(string name)
        {
            return "";
        }

        public override string AddMaxLength(int length, string errorMessage)
        {
            return $@"        [MaxLength({length})]";
        }

        public override string AddRequired(string errorMessage)
        {
            return "";
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
    }
}
