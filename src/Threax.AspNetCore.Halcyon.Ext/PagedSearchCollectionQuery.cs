using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IPagedSearchCollectionQuery : IPagedCollectionQuery, ISearchQuery
    {

    }

    public class PagedSearchCollectionQuery : IPagedSearchCollectionQuery
    {
        public int Limit { get; set; }

        public int Offset { get; set; }

        public string Term { get; set; }
    }
}
