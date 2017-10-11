using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// This interface will create label caches. It is thread safe.
    /// </summary>
    public interface ILabelCacheFactory
    {
        /// <summary>
        /// Get a label cached using type T as the key. This is the reccomended way to get your
        /// caches unless you need to use a specific object as the key. This funciton is thread safe.
        /// </summary>
        /// <typeparam name="T">The type to use as a key.</typeparam>
        /// <returns>A task that contains the cache in its result.</returns>
        Task<ILabelCache> GetLabelCache<T>();

        /// <summary>
        /// Get a label cached by the given object as a key. Use this method if you need to use specific
        /// keys. Be careful that you are passing in consistent instance or else you will leak caches.
        /// This function is thread safe.
        /// </summary>
        /// <param name="key">The key to get a cache for.</param>
        /// <returns>A task that contains the cache in its result.</returns>
        Task<ILabelCache> GetLabelCache(object key);
    }
}