using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class PagedCollectionView<T> : ICollectionView<T>, IHalLinkProvider, IQueryStringProvider
    {
        public class Rels
        {
            public const String Next = "next";
            public const String Previous = "previous";
            public const String First = "first";
            public const String Last = "last";
        }

        public PagedCollectionView(IPagedCollectionQuery query, int total, IEnumerable<T> items)
        {
            this.Items = items;
            this.Offset = query.Offset;
            this.Limit = query.Limit;
            this.Total = total;
        }

        [JsonIgnore]
        public Type CollectionType
        {
            get
            {
                return typeof(T);
            }
        }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public int Total { get; set; }

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
                if (pageLinkAttr != null)
                {
                    //Next link
                    if ((Offset + 1) * Limit < Total)
                    {
                        var builder = new QueryStringBuilder();
                        AddPageQuery(Rels.Next, builder, Offset + 1, Limit);
                        var next = builder.AddToUrl(pageLinkAttr.Href);
                        yield return new HalLinkAttribute(Rels.Next, next, null, pageLinkAttr.Method);
                    }
                    //Previous link
                    if (Offset - 1 > -1)
                    {
                        var builder = new QueryStringBuilder();
                        AddPageQuery(Rels.Previous, builder, Offset - 1, Limit);
                        var prev = builder.AddToUrl(pageLinkAttr.Href);
                        yield return new HalLinkAttribute(Rels.Previous, prev, null, pageLinkAttr.Method);
                    }

                    //First link
                    var firstBuilder = new QueryStringBuilder();
                    AddPageQuery(Rels.First, firstBuilder, 0, Limit);
                    var first = firstBuilder.AddToUrl(pageLinkAttr.Href);
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
                        var builder = new QueryStringBuilder();
                        AddPageQuery(Rels.Last, builder, lastIndex, Limit);
                        var last = builder.AddToUrl(pageLinkAttr.Href);
                        yield return new HalLinkAttribute(Rels.Last, last, null, pageLinkAttr.Method);
                    }
                }
            }
        }

        public void AddQuery(String rel, QueryStringBuilder query)
        {
            if (rel == HalSelfActionLinkAttribute.SelfRelName)
            {
                AddPageQuery(rel, query, Offset, Limit);
            }
        }

        private void AddPageQuery(String rel, QueryStringBuilder query, int? offset, int? limit)
        {
            if (offset.HasValue && limit.HasValue)
            {
                query.AppendQueryString($"offset={offset}&limit={limit}");
            }
            AddCustomQuery(rel, query);
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. The base class version just returns url, so there is no need to 
        /// call it from your subclass.
        /// </summary>
        /// <param name="rel">The input rel.</param>
        /// <param name="query">The query builder.</param>
        /// <returns>The customized query string.</returns>
        protected virtual void AddCustomQuery(String rel, QueryStringBuilder query)
        {
            
        }
    }
}
