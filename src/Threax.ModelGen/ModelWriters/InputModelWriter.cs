using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class InputModelWriter : ClassWriter
    {
        public InputModelWriter() : base(false, false)
        {
        }

        public override string AddUsings(string ns)
        {
            return $@"{base.AddUsings(ns)}
using {ns}.Models;";
        }

        public override String StartType(String name, String pluralName)
        {
            return 
$@"    [HalModel]
    public partial class {name}Input : I{name}
    {{";
        }

        public override string CreateProperty(string name, IWriterPropertyInfo info)
        {
            return 
                $@"        [UiOrder]
{base.CreateProperty(name, info)}";
        }
    }
}
