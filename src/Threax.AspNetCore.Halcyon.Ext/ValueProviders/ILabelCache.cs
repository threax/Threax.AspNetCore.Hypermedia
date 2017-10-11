using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// This is an interface for a cache of label values. These caches can be used to cache computed or 
    /// looked up label values that might take a while to generate. Implementations must be thread safe.
    /// </summary>
    public interface ILabelCache
    {
        /// <summary>
        /// Get the labels, to do this a function that returns the labels must be passed in.
        /// This funciton is thread safe.
        /// </summary>
        /// <param name="populateFunc">The funciton that generates the labels.</param>
        /// <returns>A task that returns the labels.</returns>
        Task<IEnumerable<ILabelValuePair>> GetLabels(Func<Task<List<ILabelValuePair>>> populateFunc);

        /// <summary>
        /// Clear the cached labels. Call this to make the next call to GetLabels regenerate the labels.
        /// </summary>
        /// <returns>A task that completes when the labels are cleared.</returns>
        Task Clear();
    }
}