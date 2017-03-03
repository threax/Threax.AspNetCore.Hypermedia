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
    public class CollectionView<T> : ICollectionView<T>, IHalLinkProvider, IQueryStringProvider
    {
        public class Rels
        {
            public const String Next = "next";
            public const String Previous = "previous";
            public const String First = "first";
            public const String Last = "last";
        }

        public CollectionView(IEnumerable<T> items = null)
        {
            this.Items = items;
        }

        [JsonIgnore]
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

        [JsonIgnore]
        public IEnumerable<T> Items { get; set; }

        [JsonIgnore]
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
            foreach (var attr in typeInfo.GetCustomAttributes())
            {
                //Grab the self link as the reference for page links
                var pageLinkAttr = attr as HalSelfActionLinkAttribute;
                if (pageLinkAttr != null && Offset.HasValue && Limit.HasValue && Total.HasValue)
                {
                    //Next link
                    if ((Offset + 1) * Limit < Total)
                    {
                        var next = AddPageQuery(Rels.Next, pageLinkAttr.Href, Offset + 1, Limit);
                        yield return new HalLinkAttribute(Rels.Next, next, null, pageLinkAttr.Method);
                    }
                    //Previous link
                    if (Offset - 1 > -1)
                    {
                        var prev = AddPageQuery(Rels.Previous, pageLinkAttr.Href, Offset - 1, Limit);
                        yield return new HalLinkAttribute(Rels.Previous, prev, null, pageLinkAttr.Method);
                    }

                    //First link
                    var first = AddPageQuery(Rels.First, pageLinkAttr.Href, 0, Limit);
                    yield return new HalLinkAttribute(Rels.First, first, null, pageLinkAttr.Method);

                    //Last link
                    if (Limit != 0)
                    {
                        var lastIndex = Total / Limit;
                        //If there is no remainder this is an even multiple, do not start the last page on the even multiple, but one before it
                        var remainder = Total % Limit;
                        if (remainder == 0 && lastIndex > 0)
                        {
                            --lastIndex;
                        }
                        var last = AddPageQuery(Rels.Last, pageLinkAttr.Href, lastIndex, Limit);
                        yield return new HalLinkAttribute(Rels.Last, last, null, pageLinkAttr.Method);
                    }
                }
            }
        }

        public string AddQuery(String rel, string url)
        {
            if (rel == HalSelfActionLinkAttribute.SelfRelName)
            {
                url = AddPageQuery(rel, url, Offset, Limit);
            }
            return url;
        }

        private string AddPageQuery(String rel, String url, int? offset, int? limit)
        {
            if (offset.HasValue && limit.HasValue)
            {
                url = url.AppendQueryString($"offset={offset}&limit={limit}");
            }
            return AddCustomQuery(rel, url);
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. The base class version just returns url, so there is no need to 
        /// call it from your subclass.
        /// </summary>
        /// <param name="rel">The input rel.</param>
        /// <param name="url">The url to modify.</param>
        /// <returns>The customized query string.</returns>
        protected virtual String AddCustomQuery(String rel, String url)
        {
            return url;
        }
    }
}
