using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalcyonClientOptions
    {
        /// <summary>
        /// This is called when the IHttpClientFactory is being created. You can decorate the instance
        /// with additional IHttpClientFactory instances during this callback. Be sure to return your
        /// outer decorator.
        /// If this is null, the DefaultHttpClientFactory will be used directly.
        /// </summary>
        public Func<IHttpClientFactory, IHttpClientFactory> DecorateClientFactory { get; set; } = null;
    }
}
