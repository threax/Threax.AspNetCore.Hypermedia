using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Threax.SharedHttpClient;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// The default http client factory. Most any client factory chain will end with an instance of
    /// this class. You should only need one instance of this class per application, which can live
    /// through its entire lifetime. If you use dependency injection this will be regisered as DefaultHttpClientFactory
    /// in the services, which can be recovered by anything you wish to make a chain out of.
    /// </summary>
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient GetClient()
        {
            return SharedHttpClientManager.SharedClient;
        }

        public Task<HttpRequestMessage> GetRequestMessage()
        {
            return Task.FromResult(new HttpRequestMessage());
        }
    }
}
