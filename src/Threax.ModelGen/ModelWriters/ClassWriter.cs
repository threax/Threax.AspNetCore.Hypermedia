using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class ClassWriter : ITypeWriter
    {
        private bool hasCreated = false;
        private bool hasModified = false;

        public ClassWriter(bool hasCreated, bool hasModified)
        {
            this.hasCreated = hasCreated;
            this.hasModified = hasModified;
        }

        public virtual String AddUsings(String ns)
        {
            var usings = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.UIAttrs;";

            if(hasCreated || hasModified)
            {
                usings += @"
using Threax.AspNetCore.Tracking;";
            }

            return usings;
        }

        public virtual String StartType(String name, String pluralName)
        {
            return $@"    public class {name} {GetAdditionalInterfaces()}
    {{";
        }

        public virtual String EndType(String name, String pluralName)
        {
            var sb = new StringBuilder();

            if (hasCreated)
            {
                sb.AppendLine(CreateProperty("DateTime", "Created", true));
            }

            if (hasModified)
            {
                sb.AppendLine(CreateProperty("DateTime", "Modified", true));
            }

            sb.Append("    }");
            return sb.ToString();
        }

        public virtual String AddMaxLength(int length, String errorMessage)
        {
            return $@"        [MaxLength({length}, ErrorMessage = ""{errorMessage}"")]";
        }

        public virtual String AddRequired(String errorMessage)
        {
            return $@"        [Required(ErrorMessage = ""{errorMessage}"")]";
        }

        public virtual String AddTypeDisplay(String name)
        {
            return $@"    [UiTitle(""{name}"")]";
        }

        public virtual String AddDisplay(String name)
        {
            return $@"        [Display(Name = ""{name}"")]";
        }

        public virtual String CreateProperty(String type, String name, bool isValueType)
        {
            return $"        public {type} {name} {{ get; set; }}";
        }

        public virtual string EndNamespace()
        {
            return "}";
        }

        public virtual string StartNamespace(string name)
        {
            return $@"namespace {name} 
{{";
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
