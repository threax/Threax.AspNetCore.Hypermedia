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
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Get a new http client. Calling code must dispose the instance.
        /// </summary>
        /// <returns>A new HttpClient instance to use.</returns>
        HttpClient GetClient();

        /// <summary>
        /// Get a new HttpRequestMessage. Calling code must dispose the instance.
        /// </summary>
        /// <returns></returns>
        HttpRequestMessage GetRequestMessage();
    }
}
