using System;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public interface IValueProviderResolver
    {
        bool TryGetValueProvider(Type type, out IValueProvider provider);
    }
}