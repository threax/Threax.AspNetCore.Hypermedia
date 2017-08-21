using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalEndpointClient
    {
        private IHttpClientFactory clientFactory;
        private HalLink link;
        private JObject data = null;
        private JToken links;
        private JToken embeds;

        private HalEndpointClient(HalLink link, IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.link = link;
        }

        public bool HasLink(String rel)
        {
            if(links == null)
            {
                return false;
            }
            return links[rel] != null;
        }

        public async Task<HalEndpointClient> LoadLink(String rel)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.Load(null, null);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public async Task<HalEndpointClient> LoadLinkWithQuery<QueryType>(String rel, QueryType query)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.Load(null, query);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public async Task<HalEndpointClient> LoadLinkWithBody<BodyType>(String rel, BodyType data)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.Load(data, null);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public async Task<HalEndpointClient> LoadLinkWithQueryAndBody<QueryType, BodyType>(String rel, QueryType query, BodyType data)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.Load(data, query);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public async Task<HalEndpointClient> LoadLinkWithForm<FormType>(String rel, FormType data)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.LoadWithForm(data, null);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public async Task<HalEndpointClient> LoadLinkWithQueryAndForm<QueryType, FormType>(String rel, QueryType query, FormType data)
        {
            if (links != null)
            {
                var link = links[rel];
                if (link != null)
                {
                    var client = new HalEndpointClient(link.ToObject<HalLink>(), clientFactory);
                    await client.LoadWithForm(data, query);
                    return client;
                }
            }
            throw new InvalidOperationException($"Cannot find a link named {rel}.");
        }

        public bool HasEmbed(String name)
        {
            if(embeds == null)
            {
                return false;
            }
            return embeds[name] != null;
        }

        public IEnumerable<EmbedType> GetEmbeds<EmbedType>(String name)
        {
            if(embeds != null)
            {
                var embed = embeds[name];
                if (embed != null)
                {
                    return embed.Select(i => i.ToObject<EmbedType>());
                }
            }
            throw new InvalidOperationException($"Cannot find an embed named {name}.");
        }

        public JToken GetEmbeds(String name)
        {
            if (embeds != null)
            {
                var embed = embeds[name];
                if (embed != null)
                {
                    return embed;
                }
            }
            throw new InvalidOperationException($"Cannot find an embed named {name}.");
        }

        /// <summary>
        /// Parse the data to the specified objec type. The object returned will 
        /// be a strongly typed copy of the data.
        /// </summary>
        /// <typeparam name="T">The type to convert the data to.</typeparam>
        /// <returns>The converted data.</returns>
        public T GetData<T>()
        {
            if(data != null)
            {
                return data.ToObject<T>();
            }
            return default(T);
        }

        /// <summary>
        /// Get the JObject representation of the data.
        /// </summary>
        /// <returns></returns>
        public JObject GetData()
        {
            return data;
        }

        /// <summary>
        /// The status code of the last request.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        private async Task Load(Object data, Object query)
        {
            using (var client = clientFactory.GetClient())
            {
                using (var request = clientFactory.GetRequestMessage())
                {
                    request.Method = new HttpMethod(link.Method);
                    var uriBuilder = new UriBuilder(link.Href);
                    
                    if(query != null)
                    {
                        uriBuilder.Query = QueryBuilder.BuildQueryString(query);
                    }

                    request.RequestUri = uriBuilder.Uri;

                    if (data != null)
                    {
                        request.Content = new StringContent(JsonConvert.SerializeObject(data));
                    }
                    var response = await client.SendAsync(request);
                    StatusCode = response.StatusCode;
                    if ((int)StatusCode > 299)
                    {
                        throw new InvalidOperationException($"The HTTP status code {StatusCode} is not a valid response for this client.");
                    }
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);
                    links = responseData["_links"];
                    embeds = responseData["_embedded"];
                }
            }
        }

        private async Task LoadWithForm(Object data, Object query)
        {
            using (var client = clientFactory.GetClient())
            {
                using (var request = clientFactory.GetRequestMessage())
                {
                    using(var form = new MultipartFormDataContent())
                    {
                        request.Method = new HttpMethod(link.Method);
                        var uriBuilder = new UriBuilder(link.Href);

                        if (query != null)
                        {
                            uriBuilder.Query = QueryBuilder.BuildQueryString(query);
                        }

                        request.RequestUri = uriBuilder.Uri;

                        if (data != null)
                        {
                            FormContentBuilder.BuildFormContent(data, form);
                            request.Content = form;
                        }

                        var response = await client.SendAsync(request);
                        StatusCode = response.StatusCode;
                        if ((int)StatusCode > 299)
                        {
                            throw new InvalidOperationException($"The HTTP status code {StatusCode} is not a valid response for this client.");
                        }
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseData = JObject.Parse(responseString);
                        links = responseData["_links"];
                        embeds = responseData["_embedded"];
                    }
                }
            }
        }
    }
}
