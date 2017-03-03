using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class HalClientGenOptions
    {
        public IEnumerable<Assembly> SourceAssemblies { get; set; }
    }
}
