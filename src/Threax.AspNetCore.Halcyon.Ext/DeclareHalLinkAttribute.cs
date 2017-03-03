using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// Declare a hal link attribute. This serves as a hint for the codegen to allow it
    /// to create client methods for links that are constructed dynamicly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DeclareHalLinkAttribute : Attribute
    {
        private HalRelInfo refInfo;

        /// <summary>
        /// Declare a new link with just a rel. Ignored by the results generator, but
        /// used by the code generator to make sure any dynamic functions are included
        /// in the generated clients.
        /// </summary>
        /// <param name="rel"></param>
        public DeclareHalLinkAttribute(string rel)
        {
            this.Rel = rel;
        }

        public DeclareHalLinkAttribute(string rel, Type controllerType, String[] routeArgs = null)
        {
            this.Rel = rel;
            refInfo = new HalRelInfo(rel, controllerType, routeArgs);
            if(refInfo != null)
            {
                this.GroupName = Utils.GetControllerName(refInfo.ControllerType);
                this.UriTemplate = refInfo.UrlTemplate;
                this.Method = refInfo.HttpMethod;
                LinkedToControllerRel = true;
            }
        }

        /// <summary>
        /// The rel for the declared link.
        /// </summary>
        public String Rel { get; private set; }

        /// <summary>
        /// This will be true if the declared link links to a controller rel for documentation, otherwise
        /// empty documentation is assumed, which is a plain request to the link with no args.
        /// </summary>
        public bool LinkedToControllerRel { get; private set; } = false;

        /// <summary>
        /// The "GroupName" for documentation lookup.
        /// </summary>
        public String GroupName { get; private set; }

        /// <summary>
        /// The uri template, can be used for "RelativePath" for documentation lookup.
        /// </summary>
        public String UriTemplate { get; private set; }

        /// <summary>
        /// The http method of the link.
        /// </summary>
        public String Method { get; private set; }
    }
}
