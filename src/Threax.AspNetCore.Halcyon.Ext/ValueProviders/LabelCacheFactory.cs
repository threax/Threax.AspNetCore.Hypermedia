using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class LabelCacheFactory : ILabelCacheFactory, IDisposable
    {
        private Dictionary<Object, ILabelCache> caches = new Dictionary<object, ILabelCache>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public void Dispose()
        {
            semaphore.Dispose();
            foreach(var cache in caches.Values)
            {
                var disposableCache = cache as IDisposable;
                if(disposableCache != null)
                {
                    disposableCache.Dispose();
                }
            }
        }

        public Task<ILabelCache> GetLabelCache<T>()
        {
            return GetLabelCache(typeof(T));
        }

        public async Task<ILabelCache> GetLabelCache(Object key)
        {
            ILabelCache cache;
            try
            {
                await semaphore.WaitAsync();
                if (!caches.TryGetValue(key, out cache))
                {
                    cache = new LabelCache();
                    caches.Add(key, cache);
                }
            }
            finally
            {
                semaphore.Release();
            }
            return cache;
        }
    }
}
