using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class BearerHttpClientFactory : IHttpClientFactory
    {
        private String accessToken;
        private IHttpClientFactory next;

        public BearerHttpClientFactory(String accessToken, IHttpClientFactory next)
        {
            this.accessToken = accessToken;
            this.next = next;
        }

        public void Dispose()
        {
            next.Dispose();
        }

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public async Task<HttpRequestMessage> GetRequestMessage()
        {
            var req = await next.GetRequestMessage();
            req.Headers.TryAddWithoutValidation(HeaderName, accessToken);
            return req;
        }

        public String HeaderName { get; set; } = "bearer";
    }
}
