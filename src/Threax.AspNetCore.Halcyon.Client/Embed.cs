using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class Embed
    {
        private JToken embeds;
        private IHttpClientFactory fetcher;

        public Embed(String name, JToken embeds, IHttpClientFactory fetcher)
        {
            this.Name = name;
            this.embeds = embeds;
            this.fetcher = fetcher;
        }

        public IEnumerable<HalEndpointClient> GetAllClients()
        {
            return embeds.Children().Select(i => new HalEndpointClient(i, this.fetcher));
        }

        public String Name { get; private set; }
    }
}
