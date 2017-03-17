using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISearchCollectionQuery : IPagedCollectionQuery, ISearchQuery
    {

    }

    public class SearchCollectionQuery : ISearchCollectionQuery
    {
        public int Limit { get; set; }

        public int Offset { get; set; }

        public string Term { get; set; }
    }
}
