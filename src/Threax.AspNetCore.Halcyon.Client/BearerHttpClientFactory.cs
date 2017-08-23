using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public HttpRequestMessage GetRequestMessage()
        {
            var req = next.GetRequestMessage();
            req.Headers.TryAddWithoutValidation(HeaderName, accessToken);
            return req;
        }

        public String HeaderName { get; set; } = "bearer";
    }
}
