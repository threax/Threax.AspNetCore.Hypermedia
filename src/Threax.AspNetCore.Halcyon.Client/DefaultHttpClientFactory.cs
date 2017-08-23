using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private HttpClient httpClient = new HttpClient();

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public HttpClient GetClient()
        {
            return httpClient;
        }

        public HttpRequestMessage GetRequestMessage()
        {
            return new HttpRequestMessage();
        }
    }
}
