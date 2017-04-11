using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class QueryStringBuilder
    {
        private String query;

        public QueryStringBuilder()
        {

        }

        /// <summary>
        /// Add an item to this query string, the valus will be escaped.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="value">The value.</param>
        public void AppendItem(String name, String value)
        {
            this.AppendQueryString($"{UrlEncoder.Default.Encode(name)}={UrlEncoder.Default.Encode(value)}");
        }

        /// <summary>
        /// This function will append a query string to a string url while accounting
        /// for the fact that there might be a query already. If appendQuery is null no changes are made.
        /// </summary>
        /// <param name="appendQuery">The query to append, must already be in query format, not escaped by this function.</param>
        public void AppendQueryString(String appendQuery)
        {
            if(appendQuery == null)
            {
                return;
            }

            String format = "&{0}";
            if (String.IsNullOrEmpty(query))
            {
                format = "{0}";
            }

            query += String.Format(format, appendQuery);
        }

        /// <summary>
        /// Add the query to the given url, will detect if the url already has a query in it.
        /// </summary>
        /// <param name="url">The url to add the query to.</param>
        /// <returns>The url with the query added.</returns>
        public String AddToUrl(String url)
        {
            if (url.Contains('?'))
            {
                return $"{url}&{query}";
            }
            else
            {
                return $"{url}?{query}";
            }
        }
    }
}
