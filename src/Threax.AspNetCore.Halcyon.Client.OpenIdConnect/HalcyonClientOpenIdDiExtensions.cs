using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Threax.AspNetCore.Halcyon.Client;
using Threax.AspNetCore.Halcyon.Client.OpenIdConnect;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HalcyonClientOpenIdDiExtensions
    {
        /// <summary>
        /// Add the AppTemplate setup to use client credentials to connect to the service.
        /// </summary>
        /// <param name="halClientSetup">Halcyon client setup.</param>
        /// <param name="configure">The configure callback.</param>
        /// <returns></returns>
        public static IHalcyonClientSetup<T> SetupHttpClientFactoryWithClientCredentials<T>(this IHalcyonClientSetup<T> halClientSetup, Action<OpenIdConnectClientCredentialsClientOptions> configure)
        {
            var options = new OpenIdConnectClientCredentialsClientOptions();
            configure?.Invoke(options);

            var sharedCredentials = new SharedClientCredentials();
            options.GetSharedClientCredentials?.Invoke(sharedCredentials);
            sharedCredentials.MergeWith(options.ClientCredentials);

            halClientSetup.Services.AddSingleton<IHttpClientFactory<T>>(s =>
            {
                return new ClientCredentialsAccessTokenFactory<T>(options.ClientCredentials, new BearerHttpClientFactory<T>(s.GetRequiredService<IHttpClientFactory>()));
            });

            return halClientSetup;
        }

        /// <summary>
        /// Add the AppTemplate setup to forward the current user's access token from the to the service.
        /// </summary>
        /// <param name="halClientSetup">Halcyon client setup.</param>
        /// <param name="configure">The configure callback.</param>
        /// <returns></returns>
        public static IHalcyonClientSetup<T> SetupHttpClientFactoryWithUserAccessToken<T>(this IHalcyonClientSetup<T> halClientSetup, Action<OpenIdConnectBearerClientOptions> configure)
        {
            var options = new OpenIdConnectBearerClientOptions();
            configure?.Invoke(options);

            if(options.GetAccessTokenCallback == null)
            {
                throw new InvalidOperationException("An access token callback must be provided. Use 'ClaimsPrincipalExtensions.GetAccessToken' from 'Threax.AspNetCore.AuthCore' for a good default with the rest of the framework.");
            }

            halClientSetup.Services.TryAddScoped<ILoggingInUserAccessor, LoggingInUserAccessor>();
            halClientSetup.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            halClientSetup.Services.AddScoped<IHttpClientFactory<T>>(s =>
                new AddUserTokenHttpClientFactory<T>(p => options.GetAccessTokenCallback(p), 
                                                     s.GetRequiredService<IHttpContextAccessor>(), 
                                                     s.GetRequiredService<IHttpClientFactory>(), 
                                                     s.GetRequiredService<ILoggingInUserAccessor>()));

            return halClientSetup;
        }
    }
}
