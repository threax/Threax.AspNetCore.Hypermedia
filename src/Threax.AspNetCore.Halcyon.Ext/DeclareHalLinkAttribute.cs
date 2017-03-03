using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DeclareHalLinkAttribute : Attribute
    {
        /// <summary>
        /// Declare a new link with just a rel. Ignored by the results generator, but
        /// used by the code generator to make sure any dynamic functions are included
        /// in the generated clients.
        /// </summary>
        /// <param name="rel"></param>
        public DeclareHalLinkAttribute(string rel)
        {
            this.Rel = rel;
        }

        /// <summary>
        /// The rel for the declared link.
        /// </summary>
        public String Rel { get; private set; }
    }
}
