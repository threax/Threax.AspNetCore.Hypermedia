using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalEndpointDocQuery
    {
        public bool IncludeRequest { get; set; } = true;

        public bool IncludeResponse { get; set; } = true;
    }
}
