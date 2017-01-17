using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalActionLinkAttribute : HalLinkAttribute
    {
        private Type controllerType;
        private HalRelInfo halRefInfo;

        /// <summary>
        /// Create a new link based on a controller and a function.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="controllerType"></param>
        /// <param name="actionMethod"></param>
        /// <param name="routeArgs"></param>
        /// <param name="title"></param>
        public HalActionLinkAttribute(string rel, Type controllerType, String[] routeArgs = null, string title = null)
            : this(rel, rel, controllerType, routeArgs, title)
        {
            //Everything done in other constructor
        }

        /// <summary>
        /// This constructor allows us to differentiate the real rel we pass to the base class from the one we lookup, useful for self links.
        /// </summary>
        protected HalActionLinkAttribute(string realRel, string lookupRel, Type controllerType, String[] routeArgs = null, string title = null, Object refInfoDontProvide = null)
            : base(realRel, CreateHref(lookupRel, controllerType, routeArgs, out refInfoDontProvide), title, ((HalRelInfo)refInfoDontProvide).HttpMethod)
        {
            this.controllerType = controllerType;
            halRefInfo = ((HalRelInfo)refInfoDontProvide);
        }

        public bool CanUserAccess(ClaimsPrincipal claims)
        {
            //Check for anon access attribute
            var anonAttrs = halRefInfo.ActionMethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true);
            if (anonAttrs.Any())
            {
                return true;
            }
            //Otherwise check the action method and controller for authorize attributes.
            var attributes = halRefInfo.ActionMethodInfo.GetCustomAttributes<AuthorizeAttribute>(true);
            attributes = attributes.Concat(halRefInfo.ControllerType.GetTypeInfo().GetCustomAttributes<AuthorizeAttribute>(true));
            return Utils.CheckRoles(claims, attributes);
        }

        public HalLinkAttribute GetDocLink(IHalDocEndpointInfo docEndpointInfo)
        {
            //Create a link to the endpoint info for this controller and action method.
            Object halRefObj;
            var href = CreateHref(docEndpointInfo.Rel, docEndpointInfo.ControllerType,
                new String[] {
                    $"{docEndpointInfo.GroupArg}={Utils.GetControllerName(controllerType)}",
                    $"{docEndpointInfo.MethodArg}={Method}",
                    $"{docEndpointInfo.RelativePathArg}={halRefInfo.UrlTemplate}"
                }, out halRefObj);

            var docHalRefInfo = ((HalRelInfo)halRefObj);

            return new HalLinkAttribute($"{this.Rel}.Docs", href, null, docHalRefInfo.HttpMethod);
        }

        protected static String CreateHref(String rel, Type controllerType, String[] routeArgs, out Object refInfoObj)
        {
            var refInfo = new HalRelInfo(rel, controllerType, routeArgs);
            refInfoObj = refInfo;
            return refInfo.UrlTemplate;
        }

        /// <summary>
        /// The HalRelAttribute from the endpoint this link points to.
        /// </summary>
        public HalRelAttribute HalRelAttr
        {
            get
            {
                return halRefInfo.HalRelAttr;
            }
        }

        /// <summary>
        /// Set to true to include the original request query string in the self link.
        /// </summary>
        public bool IncludeRequestQuery { get; set; }

        /// <summary>
        /// Set to true (default) to include docs for this link. Set to false to never include docs for this link.
        /// </summary>
        public bool HasDocs { get; set; } = true;
    }
}
