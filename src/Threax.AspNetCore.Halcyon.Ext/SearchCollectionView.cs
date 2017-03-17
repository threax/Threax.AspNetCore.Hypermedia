using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SearchCollectionView<T> : CollectionView<T>, IQueryStringProvider
    {
        public SearchCollectionView(SearchQuery query, IEnumerable<T> items)
            :base(items)
        {
        }

        public string AddQuery(string rel, string url)
        {
            if (rel == HalSelfActionLinkAttribute.SelfRelName && Term != null)
            {
                url = url.AppendQueryString($"term={Term}");
            }
            return url;
        }

        public String Term { get; set; }
    }
}
