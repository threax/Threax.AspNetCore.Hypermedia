using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ICollectionView
    {
        [JsonIgnore]
        String CollectionName { get; }

        [JsonIgnore]
        Type CollectionType { get; }

        [JsonIgnore]
        IEnumerable<Object> AsObjects { get; }
    }

    public interface ICollectionView<T> : ICollectionView
    {
        [JsonIgnore]
        IEnumerable<T> Items { get; }
    }

    public class CollectionView<T> : ICollectionView<T>
    {
        public CollectionView(String name = "values")
        {
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
