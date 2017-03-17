using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISearchQuery
    {
        String Term { get; set; }
    }

    public class SearchQuery : ISearchQuery
    {
        public string Term { get; set; }
    }
}
