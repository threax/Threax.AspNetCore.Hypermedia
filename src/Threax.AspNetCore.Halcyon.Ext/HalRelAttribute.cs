using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HalRelAttribute : Attribute
    {
        public HalRelAttribute(String rel)
        {
            this.Rel = rel;
        }

        public String Rel { get; private set; }

        public bool IsPaged { get; set; } = false;
    }
}
