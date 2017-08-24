using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// Options for the <see cref="ClientCredentialsAccessTokenFactory"/>
    /// </summary>
    public class ClientCredentailsAccessTokenFactoryOptions
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
        /// The scope that will be accessed.
        /// </summary>
        public String Scope { get; set; }

        /// <summary>
        /// This represents the fraction of the total lifetime of the access token to keep using it until
        /// it should be refreshed. It should be between 0 and 1, the default is 0.8 (use token for 80% of the total time, then refresh)
        /// </summary>
        public double ExpirationTimeFraction { get; set; } = 0.8d;
    }
}
