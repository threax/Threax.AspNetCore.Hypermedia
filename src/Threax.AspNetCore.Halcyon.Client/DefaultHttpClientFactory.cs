using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient GetClient()
        {
            return new HttpClient();
        }

        public HttpRequestMessage GetRequestMessage()
        {
            return new HttpRequestMessage();
        }
    }
}
