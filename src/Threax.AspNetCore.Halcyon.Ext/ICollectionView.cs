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
}
