using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class CollectionView<T> : ICollectionView<T>
    {
        public CollectionView(IEnumerable<T> items = null, String name = "values")
        {
            this.Items = items;
            this.CollectionName = name;
        }

        public string CollectionName { get; private set; }

        public Type CollectionType
        {
            get
            {
                return typeof(T);
            }
        }

        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public int? Total { get; set; }

        public IEnumerable<T> Items { get; set; }

        public IEnumerable<object> AsObjects
        {
            get
            {
                return Items.Select(i => (Object)i);
            }
        }
    }
}
