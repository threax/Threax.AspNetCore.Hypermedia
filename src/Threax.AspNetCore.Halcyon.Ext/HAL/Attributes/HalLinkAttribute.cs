using System;
using System.Collections.Generic;

namespace Halcyon.HAL.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HalLinkAttribute : Attribute
    {
        public string Rel { get; protected set; }
        public string Href { get; protected set; }
        public string Title { get; protected set; }
        public string Method { get; protected set; }
        public string DataMode { get; protected set; }

        /// <summary>
        /// Request data is additional data that can be sent to the client. This data will be included in any 
        /// request to this link, if the link is called without any user data attached. If you build up links
        /// using a RequestDataBuilder in the background the results are stored here.
        /// </summary>
        public Dictionary<String, Object> RequestData { get; protected set; }

        public HalLinkAttribute(string rel, string href, string title = null, string method = null)
        : this(rel, href, title, method, dataMode: null, requestData: null)
        {

        }

        public HalLinkAttribute(string rel, string href, string title = null, string method = null, string dataMode = null)
            : this(rel, href, title, method, dataMode: dataMode, requestData: null)
        {
            
        }

        public HalLinkAttribute(string rel, string href, string title = null, string method = null, string dataMode = null, Dictionary<String, Object> requestData = null)
        {
            Rel = rel;
            Href = href;
            Title = title;
            Method = method;
            DataMode = dataMode;
            this.RequestData = requestData;
        }

        protected HalLinkAttribute()
        {

        }
    }
}