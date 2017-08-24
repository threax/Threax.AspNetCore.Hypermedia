using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// A factory used by the HalEndpointClient to build http requests.
    /// </summary>
    public interface IHttpClientFactory : IDisposable
    {
        /// <summary>
        /// Get a the http client. DO NOT dispose it. The instance will be shared as reccomended by Microsoft.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        /// </summary>
        /// <returns>A new HttpClient instance to use.</returns>
        HttpClient GetClient();

        /// <summary>
        /// Get a new HttpRequestMessage. This might do other work, so it is async.
        /// Calling code must dispose the instance.
        /// </summary>
        /// <returns></returns>
        Task<HttpRequestMessage> GetRequestMessage();
    }
}
