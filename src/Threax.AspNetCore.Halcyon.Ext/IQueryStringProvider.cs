using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IQueryStringProvider
    {
        /// <summary>
        /// Add a query string to the passed url and return it.
        /// </summary>
        /// <param name="url">The url to add a query to.</param>
        /// <returns>The url with the query appended.</returns>
        String AddQuery(String url);
    }
}
