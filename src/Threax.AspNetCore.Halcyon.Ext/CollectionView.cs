using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text;

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

        public int? Offset { get; set; }

        public int? Limit { get; set; }

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
                        StringBuilder sb;
                        //Next link
                        if ((Offset + 1) * Limit < Total)
                        {
                            sb = new StringBuilder(halLinkAttr.Href);
                            sb.AppendQueryString($"offset={Offset + 1}&limit={Limit}");
                            yield return new HalLinkAttribute("next", sb.ToString(), null, halLinkAttr.Method);
                        }
                        //Previous link
                        if(Offset - 1 > -1)
                        {
                            sb = new StringBuilder(halLinkAttr.Href);
                            sb.AppendQueryString($"offset={Offset - 1}&limit={Limit}");
                            yield return new HalLinkAttribute("previous", sb.ToString(), null, halLinkAttr.Method);
                        }

                        sb = new StringBuilder(halLinkAttr.Href);
                        sb.AppendQueryString($"offset=0&limit={Limit}");
                        yield return new HalLinkAttribute("first", sb.ToString(), null, halLinkAttr.Method);

                        var lastIndex = Total / Limit;
                        sb = new StringBuilder(halLinkAttr.Href);
                        sb.AppendQueryString($"offset={lastIndex}&limit={Limit}");
                        yield return new HalLinkAttribute("last", sb.ToString(), null, halLinkAttr.Method);
                    }
                }
            }
        }
    }
}
