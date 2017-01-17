using Halcyon.HAL;
using Newtonsoft.Json;
using System;
using Threax.AspNetCore.Halcyon.Ext;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options for the Halcyon convention.
    /// </summary>
    public class HalcyonConventionOptions
    {
        public const String HostVariable = "{{host}}";

        /// <summary>
        /// Set the base url to use when creating links. You can add the variable {{host}} to the string
        /// to replace that path with the current Request.GetDisplayUrl().Authority value. Otherwise you can
        /// set the url to whatever you want.
        /// </summary>
        public string BaseUrl { get; set; } = HostVariable;

        /// <summary>
        /// Provides info about the controller endpoint that exposes documentation. Can be null to not support documentation.
        /// </summary>
        public IHalDocEndpointInfo HalDocEndpointInfo { get; set; } = null;
    }
}