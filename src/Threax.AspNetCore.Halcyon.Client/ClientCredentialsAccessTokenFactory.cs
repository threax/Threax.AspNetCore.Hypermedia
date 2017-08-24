using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// This factory will lookup client credentials from the id server and use that as the access token
    /// when creating request messages. It must be disposed to release its inner semaphore. Ideally register
    /// it in your di system with TRef set to the generated halcyon entry point injector that will consume the instance.
    /// </summary>
    /// <typeparam name="TRef">
    /// TRef allows different instances of this factory to be registered
    /// for different halcyon entry points if needed. TRef is never used by this class.
    /// If in doubt use the injector class this will end up a part of.
    /// </typeparam>
    public class ClientCredentialsAccessTokenFactory<TRef> : IHttpClientFactory, IDisposable
    {
        private BearerHttpClientFactory next;
        private DateTime jwtExpiration;
        private DiscoveryResponse discoveryClient;
        private ClientCredentailsAccessTokenFactoryOptions options;
        private SemaphoreSlim refreshLocker = new SemaphoreSlim(1, 1);

        public ClientCredentialsAccessTokenFactory(ClientCredentailsAccessTokenFactoryOptions options)
        {
            if (options.ExpirationTimeFraction < 0.0d || options.ExpirationTimeFraction > 1.0d)
            {
                throw new InvalidOperationException($"The specified ExpirationTimeFraction is not between 0 and 1.");
            }

            this.options = options;
            this.next = options.BearerHttpClientFactory;
        }

        public void Dispose()
        {
            refreshLocker.Dispose();
        }

        public HttpClient GetClient()
        {
            return next.GetClient();
        }

        public async Task<HttpRequestMessage> GetRequestMessage()
        {
            //Wait on a semaphore to check the status, we only want to refresh the access token
            //once if multiple threads attempt to make a request when we need to refresh. This
            //will limit the throughput of this part of the function to just 1 thread at a time,
            //but the checks are pretty basic and should finish quickly. If you are seeing performance
            //problems because of this factory, this is likely why
            await refreshLocker.WaitAsync();

            try
            {
                if (discoveryClient == null)
                {
                    // discover endpoints from metadata
                    discoveryClient = await DiscoveryClient.GetAsync(options.IdServerHost);
                    await RefreshToken();
                }
                else if (DateTime.UtcNow > jwtExpiration)
                {
                    await RefreshToken();
                }
            }
            finally
            {
                refreshLocker.Release();
            }

            return await next.GetRequestMessage();
        }

        private async Task RefreshToken()
        {
            using (var tokenClient = new TokenClient(discoveryClient.TokenEndpoint, options.ClientId, options.ClientSecret))
            {
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(options.Scope);
                if (tokenResponse.IsError)
                {
                    throw new InvalidOperationException($"{tokenResponse.Error} Error logging into id server '{options.IdServerHost}'. Message: {tokenResponse.ErrorDescription}");
                }

                //This is not for validation of any kind, just need a way to get the expiration date
                var jwt = new JwtSecurityTokenHandler();
                var token = jwt.ReadJwtToken(tokenResponse.AccessToken);

                var expirationTimeSpan = token.ValidTo - token.ValidFrom;
                var almostExpiredTime = expirationTimeSpan.TotalSeconds * options.ExpirationTimeFraction;
                this.jwtExpiration = token.ValidTo + TimeSpan.FromSeconds(almostExpiredTime);

                next.AccessToken = tokenResponse.AccessToken;
            }
        }
    }
}
