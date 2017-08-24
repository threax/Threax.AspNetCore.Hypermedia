using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// Send an access token under a "bearer" (default) or other header. Be careful to only make
    /// https requests using this or else you could leak access tokens.
    /// </summary>
    public class BearerHttpClientFactory<TRef> : IHttpClientFactory<TRef>
    {
        private IHttpClientFactory next;

        public BearerHttpClientFactory(IHttpClientFactory next)
        {
            this.next = next;
        }

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public async Task<HttpRequestMessage> GetRequestMessage()
        {
            var req = await next.GetRequestMessage();
            req.Headers.TryAddWithoutValidation(HeaderName, AccessToken);
            return req;
        }

        /// <summary>
        /// The name of the header, defaults to "bearer"
        /// </summary>
        public String HeaderName { get; set; } = "bearer";

        /// <summary>
        /// The access token, defaults to null.
        /// </summary>
        public String AccessToken { get; set; }
    }
}
