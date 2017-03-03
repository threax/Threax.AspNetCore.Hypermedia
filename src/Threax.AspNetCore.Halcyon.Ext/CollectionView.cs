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
    [DeclareHalLink(CollectionView<Object>.Rels.Next)]
    [DeclareHalLink(CollectionView<Object>.Rels.Previous)]
    [DeclareHalLink(CollectionView<Object>.Rels.First)]
    [DeclareHalLink(CollectionView<Object>.Rels.Last)]
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
                            var next = AddPageQuery(Rels.Next, halLinkAttr.Href, Offset + 1, Limit);
                            yield return new HalLinkAttribute(Rels.Next, next, null, halLinkAttr.Method);
                        }
                        //Previous link
                        if(Offset - 1 > -1)
                        {
                            var prev = AddPageQuery(Rels.Previous, halLinkAttr.Href, Offset - 1, Limit);
                            yield return new HalLinkAttribute(Rels.Previous, prev, null, halLinkAttr.Method);
                        }

                        //First link
                        var first = AddPageQuery(Rels.First, halLinkAttr.Href, 0, Limit);
                        yield return new HalLinkAttribute(Rels.First, first, null, halLinkAttr.Method);

                        //Last link
                        if (Limit != 0)
                        {
                            var lastIndex = Total / Limit;
                            //If there is no remainder this is an even multiple, do not start the last page on the even multiple, but one before it
                            var remainder = Total % Limit;
                            if(remainder == 0 && lastIndex > 0)
                            {
                                --lastIndex;
                            }
                            var last = AddPageQuery(Rels.Last, halLinkAttr.Href, lastIndex, Limit);
                            yield return new HalLinkAttribute(Rels.Last, last, null, halLinkAttr.Method);
                        }
                    }
                }
            }
        }

        public string AddQuery(String rel, string url)
        {
            if(rel == HalSelfActionLinkAttribute.SelfRelName)
            {
                url = AddPageQuery(rel, url, Offset, Limit);
            }
            return url;
        }

        private string AddPageQuery(String rel, String url, int? offset, int? limit)
        {
            if(offset.HasValue && limit.HasValue)
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
