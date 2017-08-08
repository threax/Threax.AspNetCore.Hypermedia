using System;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaCustomizerResolver
    {
        bool TryGetValueProvider(Type type, out ISchemaCustomizer provider);
    }
}