using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class IdInterfaceWriter : InterfaceWriter
    {
        public override string EndType(String name)
        {
            return $@"
{base.EndType(name)}

    public interface I{name}Id
    {{
        Guid {name}Id {{ get; set; }}
    }}";
        }
    }
}

