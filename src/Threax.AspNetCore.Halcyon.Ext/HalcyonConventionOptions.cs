using Halcyon.HAL;
using Newtonsoft.Json;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options for the Halcyon convention.
    /// </summary>
    public class HalcyonConventionOptions
    {
        public const String AuthorityVariable = "{{authority}}";

        /// <summary>
        /// Set the base url to use when creating links. You can add the variable {{authority}} to the string
        /// to replace that path with the current Request.GetDisplayUrl().Authority value. Otherwise you can
        /// set the url to whatever you want.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}