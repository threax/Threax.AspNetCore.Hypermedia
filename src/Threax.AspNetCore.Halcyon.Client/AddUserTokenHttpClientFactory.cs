using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// This http client factory will take an access token from the ClaimsPrincipal. This class will only resolve
    /// the source of the claims principal, it relies on a user supplied token retriever function to actually
    /// get the access token. It can optionally use a ILoggingInUserAccessor to get access to the user while they
    /// are still on their login request. It is up to the external login process to set the claims principal on
    /// the instance of that interface passed to this function before using the factory.
    /// </summary>
    /// <typeparam name="TRef"></typeparam>
    public class AddUserTokenHttpClientFactory<TRef> : IHttpClientFactory<TRef>
    {
        private IHttpContextAccessor httpContextAccessor;
        private AddHeaderHttpClientFactory<TRef> next;

        public AddUserTokenHttpClientFactory(Func<ClaimsPrincipal, String> userTokenRetriever, IHttpContextAccessor httpContextAccessor, IHttpClientFactory next, ILoggingInUserAccessor loggingInUserAccessor = null)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.next = new AddHeaderHttpClientFactory<TRef>("bearer", () =>
            {
                String token = null;
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    //If the user is authenticated, use its access token
                    token = userTokenRetriever(httpContextAccessor.HttpContext.User);
                }
                else if (loggingInUserAccessor != null && loggingInUserAccessor.Principal != null)
                {
                    //The user might be logging in still, so use that access token
                    token = userTokenRetriever(loggingInUserAccessor.Principal);
                }

                return token;
            }, next);
        }

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public Task<HttpRequestMessage> GetRequestMessage()
        {
            return next.GetRequestMessage();
        }
    }
}
