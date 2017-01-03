using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalLink
    {
        public HalLink(String href = null, String method = "GET")
        {
            this.Href = href;
            this.Method = method;
        }

        public String Href { get; set; }

        public String Method { get; set; }
    }
}
