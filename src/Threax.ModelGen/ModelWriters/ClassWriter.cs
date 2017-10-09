using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class ClassWriter : ITypeWriter
    {
        public virtual String AddUsings(String ns)
        {
            return @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.UIAttrs;";
        }

        public virtual String StartType(String name, String pluralName)
        {
            return $@"    public class {name} 
    {{";
        }

        public virtual String EndType(String name, String pluralName)
        {
            return "    }";
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

        public virtual String CreateProperty(String type, String name)
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
    }
}
