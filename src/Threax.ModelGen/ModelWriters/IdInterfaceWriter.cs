using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class IdInterfaceWriter : InterfaceWriter
    {
        public IdInterfaceWriter() : base(false, false)
        {
        }

        public override string EndType(String name, String pluralName)
        {
            return $@"
{base.EndType(name, pluralName)}

    public partial interface I{name}Id
    {{
        Guid {name}Id {{ get; set; }}
    }}    

    public partial interface I{name}Query
    {{
        Guid? {name}Id {{ get; set; }}
    }}";
        }
    }
}

