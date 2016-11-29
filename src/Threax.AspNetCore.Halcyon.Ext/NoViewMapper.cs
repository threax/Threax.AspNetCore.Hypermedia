using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This mapper just returns the original objects.
    /// </summary>
    public class NoViewMapper : IHalModelViewMapper
    {
        public object Convert(object src)
        {
            return src;
        }
    }
}
