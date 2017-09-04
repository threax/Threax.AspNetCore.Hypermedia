using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class InterfaceWriter : ClassWriter
    {
        public override String StartType(String name)
        {
            return $@"    public interface I{name} 
    {{";
        }

        public override String CreateProperty(String type, String name)
        {
            return $"        {type} {name} {{ get; set; }}";
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
            return "";
        }

        public override string AddRequired(string errorMessage)
        {
            return "";
        }
    }
}
