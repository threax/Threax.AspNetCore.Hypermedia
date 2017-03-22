using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// Value providers should be injected, this will resolve them from the service collection.
    /// </summary>
    public class ValueProviderResolver : IValueProviderResolver
    {
        IServiceProvider services;

        public ValueProviderResolver(IServiceProvider services)
        {
            this.services = services;
        }

        public bool TryGetValueProvider(Type type, out IValueProvider provider)
        {
            provider = services.GetService(type) as IValueProvider;
            return provider != null;
        }
    }
}
