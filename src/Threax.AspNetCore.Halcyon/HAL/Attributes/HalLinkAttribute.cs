using System;

namespace Halcyon.HAL.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HalLinkAttribute : Attribute {
        public string Rel { get; protected set; }
        public string Href { get; protected set; }
        public string Title { get; protected set; }
        public string Method { get; protected set; }

        public HalLinkAttribute(string rel, string href, string title = null, string method = null) {
            Rel = rel;
            Href = href;
            Title = title;
            Method = method;
        }

        protected HalLinkAttribute()
        {
            
        }
    }
}