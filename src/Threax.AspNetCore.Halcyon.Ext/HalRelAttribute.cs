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

        //Can this be done another way, perhaps identify the arguments offset and limit
        public bool IsPaged { get; set; } = false;
    }
}
