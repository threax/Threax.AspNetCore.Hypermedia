using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client.OpenIdConnect
{
    /// <summary>
    /// Client credentials settings that can be shared.
    /// </summary>
    public class SharedClientCredentials
    {
        /// <summary>
        /// The host for the identity server providing tokens.
        /// </summary>
        public String IdServerHost { get; set; }

        /// <summary>
        /// The client id to login with.
        /// </summary>
        public String ClientId { get; set; }

        /// <summary>
        /// The secret for the client id.
        /// </summary>
        public String ClientSecret { get; set; }

        /// <summary>
        /// Copy any settings from this class to the passed in options. Any setttings that are null
        /// will be copied onto the options.
        /// </summary>
        /// <param name="clientCredentails">The target object.</param>
        public void MergeWith(ClientCredentailsAccessTokenFactoryOptions clientCredentails)
        {
            clientCredentails.IdServerHost = clientCredentails.IdServerHost ?? this.IdServerHost;
            clientCredentails.ClientId = clientCredentails.ClientId ?? this.ClientId;
            clientCredentails.ClientSecret = clientCredentails.ClientSecret ?? this.ClientSecret;
        }
    }
}
