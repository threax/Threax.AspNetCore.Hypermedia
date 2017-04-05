using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class EndpointClientLinkDefinition
    {
        public EndpointClientLinkDefinition(String rel, EndpointDoc doc, bool docsOnly)
        {
            this.Rel = rel;
            this.EndpointDoc = doc;
            this.DocsOnly = docsOnly;
        }

        public String Rel { get; private set; }

        public bool DocsOnly { get; private set; }

        public EndpointDoc EndpointDoc { get; private set; }
    }
}
