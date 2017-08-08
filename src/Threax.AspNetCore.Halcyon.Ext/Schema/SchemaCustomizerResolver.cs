using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// Schema customizers should be injected, this will resolve them from the service collection.
    /// </summary>
    public class SchemaCustomizerResolver : ISchemaCustomizerResolver
    {
        IServiceProvider services;

        public SchemaCustomizerResolver(IServiceProvider services)
        {
            this.services = services;
        }

        public bool TryGetValueProvider(Type type, out ISchemaCustomizer provider)
        {
            provider = services.GetService(type) as ISchemaCustomizer;
            return provider != null;
        }
    }
}
