using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client.OpenIdConnect
{
    public class OpenIdConnectClientCredentialsClientOptions
    {
        /// <summary>
        /// The options when using ClientCredentials, otherwise ignored.
        /// </summary>
        public ClientCredentailsAccessTokenFactoryOptions ClientCredentials { get; set; } = new ClientCredentailsAccessTokenFactoryOptions();

        /// <summary>
        /// If ClientCredentials is null and this is set to a function the client credentials setup by the callback will be used.
        /// </summary>
        [JsonIgnore]
        public Action<SharedClientCredentials> GetSharedClientCredentials { get; set; }
    }
}
