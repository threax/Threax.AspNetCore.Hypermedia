using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Threax.AspNetCore.AuthCore;

namespace Threax.AspNetCore.Halcyon.Client.OpenIdConnect
{
    public class OpenIdConnectBearerClientOptions
    {
        /// <summary>
        /// A callback to get the access token. Defaults to the 
        /// ClaimsPrincipalExtensions.GetAccessToken function from 
        /// Threax.Auth.Core, but can be set to anything.
        /// </summary>
        public Func<ClaimsPrincipal, string> GetAccessTokenCallback { get; set; } = ClaimsPrincipalExtensions.GetAccessToken;
    }
}
