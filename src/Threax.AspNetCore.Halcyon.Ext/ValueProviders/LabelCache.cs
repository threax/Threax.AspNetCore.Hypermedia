using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class LabelCache : ILabelCache, IDisposable
    {
        private List<ILabelValuePair> cahcedLabels;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public void Dispose()
        {
            semaphore.Dispose();
        }

        public async Task<IEnumerable<ILabelValuePair>> GetLabels(Func<Task<List<ILabelValuePair>>> populateFunc)
        {
            try
            {
                await semaphore.WaitAsync();
                if(cahcedLabels == null)
                {
                    cahcedLabels = await populateFunc();
                }
                return cahcedLabels;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task Clear()
        {
            try
            {
                await semaphore.WaitAsync();
                cahcedLabels = null;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
