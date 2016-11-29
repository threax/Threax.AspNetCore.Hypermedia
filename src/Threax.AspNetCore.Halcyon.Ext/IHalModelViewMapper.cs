using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IHalModelViewMapper
    {
        /// <summary>
        /// Convert a passed in model to a view model. How this is done is up to the implementors.
        /// </summary>
        /// <param name="src">The source object</param>
        /// <returns>The converted object.</returns>
        Object Convert(Object src);
    }
}
