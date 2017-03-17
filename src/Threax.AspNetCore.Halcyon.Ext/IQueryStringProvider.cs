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
        /// <param name="rel">The current rel.</param>
        /// <param name="query">The query builder.</param>
        /// <returns>The url with the query appended.</returns>
        void AddQuery(String rel, QueryStringBuilder query);
    }
}
