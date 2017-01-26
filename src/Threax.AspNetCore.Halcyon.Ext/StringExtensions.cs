using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public static class StringExtensions
    {
        /// <summary>
        /// This function will append a query string to a string url while accounting
        /// for the fact that there might be a query already.
        /// </summary>
        /// <param name="build">The string builder to append to.</param>
        /// <param name="appendQuery">The query to append, must already be in query format, not escaped by this function.</param>
        public static String AppendQueryString(this String build, String appendQuery)
        {
            if (!build.Contains('?'))
            {
                build += String.Format("?{0}", appendQuery);
            }
            else
            {
                build += String.Format("&{0}", appendQuery);
            }

            return build;
        }
    }
}
