using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace Threax.SharedHttpClient
{
    /// <summary>
    /// This class contains a single HttpClient. It allows you to gain access to this client 2 ways.
    /// The first is to use the shared client property to access the shared instance directly. The other is
    /// to call AddThreaxSharedHttpClient on your dependency injector, which is the reccomended approach. This 
    /// will register an ISharedHttpClient instance that contains the same HttpClient that this class returns through
    /// SharedClient. You can also inject the SharedClient instance into your own dependency injector if needed.
    /// Everything will use the same instance.
    /// </summary>
    public static class SharedHttpClientManager
    {
        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// The shared static instance of HttpClient that this library manages. This instance is used here and in the ISharedHttpClient
        /// instances. Be careful to only use the thread safe functions of HttpClient. Those methods are CancelPendingRequests, DeleteAsync
        /// GetAsync, GetByteArrayAsync, GetStreamAsync, GetStringAsync, PostAsync, PutAsync, SendAsync. This is according to 
        /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?redirectedfrom=MSDN&amp;view=netframework-4.7.2#remarks
        /// </summary>
        public static HttpClient SharedClient { get => httpClient; }

        /// <summary>
        /// Register ISharedHttpClient in the given service collection. This will use TryAdd, so if the service was already
        /// registerd it will not be registered again.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddThreaxSharedHttpClient(this IServiceCollection services)
        {
            return SharedHttpClientManager.AddThreaxSharedHttpClient(services, null);
        }

        /// <summary>
        /// Register ISharedHttpClient in the given service collection. This will use TryAdd, so if the service was already
        /// registerd it will not be registered again.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">A configuration callback.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddThreaxSharedHttpClient(this IServiceCollection services, Action<SharedHttpClientOptions> configure)
        {
            var options = new SharedHttpClientOptions();
            configure?.Invoke(options);

            services.TryAddSingleton<ISharedHttpClient, DefaultSharedHttpClient>();

            if (options.RegisterHttpClientDirectly)
            {
                services.TryAddSingleton<HttpClient>(s => SharedHttpClientManager.SharedClient);
            }

            return services;
        }
    }
}
