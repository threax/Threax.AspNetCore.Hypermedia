﻿using Microsoft.AspNetCore.Http;
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
        /// <param name="services">The service collection.</param>
        /// <param name="configure">The configure callback.</param>
        /// <returns></returns>
        public static IHalcyonClientSetup SetupHttpClientFactoryWithClientCredentials<T>(this IHalcyonClientSetup halClientSetup, Action<OpenIdConnectClientCredentialsClientOptions> configure)
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
        /// <param name="services">The service collection.</param>
        /// <param name="configure">The configure callback.</param>
        /// <returns></returns>
        public static IHalcyonClientSetup SetupHttpClientFactoryWithUserAccessToken<T>(this IHalcyonClientSetup halClientSetup, Action<OpenIdConnectBearerClientOptions> configure)
        {
            var options = new OpenIdConnectBearerClientOptions();
            configure?.Invoke(options);

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
