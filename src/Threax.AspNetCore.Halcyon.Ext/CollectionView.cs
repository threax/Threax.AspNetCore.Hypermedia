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
    public class CollectionView<T> : ICollectionView<T>
    {
        public CollectionView(IEnumerable<T> items)
        {
            this.Items = items;
        }

        [JsonIgnore]
        public Type CollectionType
        {
            get
            {
                return typeof(T);
            }
        }

        [JsonIgnore]
        public IEnumerable<T> Items { get; set; }

        [JsonIgnore]
        public IEnumerable<object> AsObjects
        {
            get
            {
                return Items.Select(i => (Object)i);
            }
        }
    }
}
