using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class EndpointClientDefinition
    {
        public EndpointClientDefinition(String name)
        {
            this.Name = name;
        }

        public String Name { get; }

        public List<EndpointClientLinkDefinition> Links { get; set; } = new List<EndpointClientLinkDefinition>();
    }
}
