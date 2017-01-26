using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class CollectionView<T> : ICollectionView<T>, IHalLinkProvider
    {
        public CollectionView(IEnumerable<T> items = null, String name = "values")
        {
            this.Items = items;
            this.CollectionName = name;
        }

        public string CollectionName { get; private set; }

        public Type CollectionType
        {
            get
            {
                return typeof(T);
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Offset { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Total { get; set; }

        public IEnumerable<T> Items { get; set; }

        public IEnumerable<object> AsObjects
        {
            get
            {
                return Items.Select(i => (Object)i);
            }
        }

        public virtual IEnumerable<HalLinkAttribute> CreateHalLinks(ILinkProviderContext context)
        {
            var typeInfo = this.GetType().GetTypeInfo();
            foreach(var attr in typeInfo.GetCustomAttributes())
            {
                //Get the HalActionLinkAttributes, but ignore the self links
                var halLinkAttr = attr as HalActionLinkAttribute;
                if(halLinkAttr != null && !(halLinkAttr is HalSelfActionLinkAttribute))
                {
                    if (halLinkAttr.HalRelAttr.IsPaged && Offset.HasValue && Limit.HasValue && Total.HasValue)
                    {
                        //Next link
                        if ((Offset + 1) * Limit < Total)
                        {
                            var next = AddPageQuery(halLinkAttr.Href, Offset + 1, Limit);
                            yield return new HalLinkAttribute("next", next, null, halLinkAttr.Method);
                        }
                        //Previous link
                        if(Offset - 1 > -1)
                        {
                            var prev = AddPageQuery(halLinkAttr.Href, Offset - 1, Limit);
                            yield return new HalLinkAttribute("previous", prev, null, halLinkAttr.Method);
                        }

                        //First link
                        var first = AddPageQuery(halLinkAttr.Href, 0, Limit);
                        yield return new HalLinkAttribute("first", first, null, halLinkAttr.Method);

                        //Last link
                        if (Limit != 0)
                        {
                            var lastIndex = Total / Limit;
                            var last = AddPageQuery(halLinkAttr.Href, lastIndex, Limit);
                            yield return new HalLinkAttribute("last", last, null, halLinkAttr.Method);
                        }
                    }
                }
            }
        }

        private string AddPageQuery(String url, int? offset, int? limit)
        {
            if(offset.HasValue && limit.HasValue)
            {
                url = url.AppendQueryString($"offset={offset}&limit={limit}");
            }
            return AddCustomQuery(url);
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. The base class version just returns url, so there is no need to 
        /// call it from your subclass.
        /// </summary>
        /// <param name="url">The url to modify.</param>
        /// <returns>The customized query string.</returns>
        protected virtual String AddCustomQuery(String url)
        {
            return url;
        }
    }
}
