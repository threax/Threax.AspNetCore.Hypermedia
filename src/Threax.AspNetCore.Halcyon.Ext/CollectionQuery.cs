using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// An interface to get the collection args from a query object.
    /// </summary>
    public interface ICollectionQuery
    {
        /// <summary>
        /// The number of pages (item number = Offset * Limit) into the collection to query.
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// The limit of the number of items to return.
        /// </summary>
        int Limit { get; set; }
    }

    /// <summary>
    /// Default implementation of ICollectionQuery.
    /// </summary>
    public class CollectionQuery
    {
        /// <summary>
        /// The number of pages (item number = Offset * Limit) into the collection to query.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// The limit of the number of items to return.
        /// </summary>
        public int Limit { get; set; } = 10;
    }
}
