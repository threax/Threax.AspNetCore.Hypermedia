using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SearchCollectionView<T> : PagedCollectionView<T>
    {
        public SearchCollectionView(ISearchCollectionQuery query, int total, IEnumerable<T> items)
            :base(query, total, items)
        {
            this.Term = query.Term;
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. You should call url = base.AddCustomQuery(rel, url) at some point if you
        /// override this function.
        /// </summary>
        /// <param name="rel">The input rel.</param>
        /// <param name="url">The url to modify.</param>
        /// <returns>The customized query string.</returns>
        protected override String AddCustomQuery(String rel, String url)
        {
            url = base.AddCustomQuery(rel, url);
            if (Term != null)
            {
                url = url.AppendQueryString($"term={Term}");
            }
            return url;
        }

        public String Term { get; set; }
    }
}
