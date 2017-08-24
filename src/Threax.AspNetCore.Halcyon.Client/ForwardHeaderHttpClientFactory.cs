using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Client;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// This class will forward a header from the incoming request to the HttpRequestMessages it creates.
    /// It will need to be registered as scoped in order for this to work. If the header is not found in the 
    /// request it can optionally throw an exception. By default the class is setup to forward a "bearer" header
    /// and to throw if the header is not found. This makes it easy to use to forward access tokens out of the box, 
    /// but it can forward any header. You can chain these together to forward multiple headers if desired.
    /// </summary>
    /// <typeparam name="TRef"></typeparam>
    public class ForwardHeaderHttpClientFactory<TRef> : IHttpClientFactory<TRef>
    {
        private IHttpClientFactory next;
        private IHttpContextAccessor httpContextAccessor;

        public ForwardHeaderHttpClientFactory(IHttpClientFactory next, IHttpContextAccessor httpContextAccessor)
        {
            this.next = next;
            this.httpContextAccessor = httpContextAccessor;
        }

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public async Task<HttpRequestMessage> GetRequestMessage()
        {
            var req = await next.GetRequestMessage();
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderName, out StringValues headerValues))
            {
                req.Headers.TryAddWithoutValidation(HeaderName, headerValues.First());
            }
            else if(ThrowOnMissingHeader)
            {
                throw new InvalidOperationException($"A header named '{HeaderName}' could not be found in the request. Cannot forward it.");
            }
            return req;
        }

        /// <summary>
        /// The name of the header to forward, defaults to "bearer".
        /// </summary>
        public String HeaderName { get; set; } = "bearer";

        /// <summary>
        /// Set to true (default) to throw an InvalidOperationException if the request does not contain the header.
        /// </summary>
        public bool ThrowOnMissingHeader { get; set; } = true;
    }
}
