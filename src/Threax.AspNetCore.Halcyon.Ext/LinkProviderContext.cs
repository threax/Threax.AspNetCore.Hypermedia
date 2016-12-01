using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class LinkProviderContext : ILinkProviderContext
    {
        public LinkProviderContext(HttpContext httpContext)
        {
            this.HttpContext = httpContext;
        }

        public HttpContext HttpContext { get; set; }
    }
}
