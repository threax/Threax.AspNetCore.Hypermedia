using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HalcyonClientDiExtensions
    {
        /// <summary>
        /// Add the halcyon client services, this will register IHttpClientFactory as a singleton.
        /// You can decorate it as needed by supplying a configureOptions callback. This will also register
        /// the DefaultHttpClientFactory as a singleton, which will handle its disposal automatically.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddHalcyonClient(this IServiceCollection services, Action<HalcyonClientOptions> configureOptions = null)
        {
            var options = new HalcyonClientOptions();
            configureOptions?.Invoke(options);

            services.TryAddSingleton<DefaultHttpClientFactory>();
            services.TryAddSingleton<IHttpClientFactory>(s =>
            {
                IHttpClientFactory factory = s.GetRequiredService<DefaultHttpClientFactory>();
                if (options.DecorateClientFactory != null)
                {
                    factory = options.DecorateClientFactory(factory);
                }
                return factory;
            });

            return services;
        }
    }
}
