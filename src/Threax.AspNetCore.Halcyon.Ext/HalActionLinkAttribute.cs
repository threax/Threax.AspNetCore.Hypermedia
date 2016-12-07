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
        private HalRefInfo halRefInfo;

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
            : base(realRel, CreateHref(lookupRel, controllerType, routeArgs, out refInfoDontProvide), title, ((HalRefInfo)refInfoDontProvide).HttpMethod)
        {
            this.controllerType = controllerType;
            halRefInfo = ((HalRefInfo)refInfoDontProvide);
        }

        public bool CanUserAccess(ClaimsPrincipal claims)
        {
            return Utils.CheckRoles(claims, halRefInfo.ActionMethodInfo.GetCustomAttributes<AuthorizeAttribute>(true));
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

            var docHalRefInfo = ((HalRefInfo)halRefObj);

            return new HalLinkAttribute($"{this.Rel}.Docs", href, null, docHalRefInfo.HttpMethod);
        }

        protected static String CreateHref(String rel, Type controllerType, String[] routeArgs, out Object refInfoObj)
        {
            var refInfo = new HalRefInfo(rel, controllerType, routeArgs);
            refInfoObj = refInfo;
            return refInfo.UrlTemplate;
        }
    }
}
