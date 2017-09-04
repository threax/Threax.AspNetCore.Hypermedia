using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class InputModelWriter : ClassWriter
    {
        public override string AddUsings(string ns)
        {
            return $@"{base.AddUsings(ns)}
using {ns}.Models;";
        }

        public override String StartType(String name)
        {
            return 
$@"    [HalModel]
    public class {name}Input : I{name}
    {{";
        }
    }
}
