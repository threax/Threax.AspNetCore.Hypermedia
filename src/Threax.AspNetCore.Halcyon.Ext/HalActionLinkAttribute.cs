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
        /// Create a new link based on a controller and a function. This will lookup the rel from the linked function. Use nameof to send
        /// the funcName string to stay strongly typed.
        /// </summary>
        /// <param name="controllerType">The controller type to lookup the rel on.</param>
        /// /// <param name="funcName">The function on the controller to lookup.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">Title for the link.</param>
        public HalActionLinkAttribute(Type controllerType, String funcName, String[] routeArgs = null, string title = null)
        {
            this.halRefInfo = new HalRelInfo(controllerType, funcName, routeArgs);
            this.Rel = this.halRefInfo.HalRelAttr.Rel;
            this.Href = this.halRefInfo.UrlTemplate;
            this.Title = title;
            this.Method = this.halRefInfo.HttpMethod;
            this.controllerType = controllerType;
        }

        /// <summary>
        /// Create a new link based on a controller and a function. The rel used is specified by publicRel, but the rest of the info will
        /// be discovered from the controller function itself. Use nameof to send the name of the funciton you want to connect to.
        /// </summary>
        /// <param name="publicRel">The rel to use as the rel for this link. Will replace the rel defined on the function.</param>
        /// <param name="controllerType">The controller type to lookup the rel on.</param>
        /// <param name="funcName">The function on the controller to lookup.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">Title for the link.</param>
        public HalActionLinkAttribute(Type controllerType, String funcName, String publicRel, String[] routeArgs = null, string title = null)
            :this(controllerType, funcName, routeArgs, title)
        {
            this.Rel = publicRel; //Replace the discovered rel with the public rel the user provided.
        }

        /// <summary>
        /// Create a new link based on a controller and a function.
        /// </summary>
        /// <param name="rel">The rel on the controller to lookup.</param>
        /// <param name="controllerType">The controller type to lookup the rel on.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">Title for the link.</param>
        public HalActionLinkAttribute(string rel, Type controllerType, String[] routeArgs = null, string title = null)
            : this(rel, rel, controllerType, routeArgs, title)
        {
            //Everything done in other constructor
        }

        /// <summary>
        /// Remap a rel to a different name, useful for links that need a common name.
        /// </summary>
        /// <param name="realRel">The rel to display to the world.</param>
        /// <param name="lookupRel">The rel on the controller to lookup.</param>
        /// <param name="controllerType">The controller type to lookup the rel on.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">Title for the link.</param>
        public HalActionLinkAttribute(string realRel, string lookupRel, Type controllerType, String[] routeArgs = null, string title = null)
        {
            this.ConstructFromRels(realRel, lookupRel, controllerType, routeArgs, title);
        }

        /// <summary>
        /// This constructor allows us to differentiate the real rel we pass to the base class from the one we lookup, useful for self links.
        /// </summary>
        private void ConstructFromRels(string realRel, string lookupRel, Type controllerType, String[] routeArgs = null, string title = null)
        {
            this.halRefInfo = new HalRelInfo(lookupRel, controllerType, routeArgs);
            this.Rel = realRel;
            this.Href = this.halRefInfo.UrlTemplate;
            this.Title = title;
            this.Method = this.halRefInfo.HttpMethod;
            this.controllerType = controllerType;
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
            var docHalRefInfo = new HalRelInfo(docEndpointInfo.Rel, docEndpointInfo.ControllerType,
                new String[] {
                    $"{docEndpointInfo.GroupArg}={GroupName}",
                    $"{docEndpointInfo.MethodArg}={Method}",
                    $"{docEndpointInfo.RelativePathArg}={UriTemplate.TrimStart('\\', '/')}"
                });

            return new HalLinkAttribute($"{this.Rel}.Docs", docHalRefInfo.UrlTemplate, null, docHalRefInfo.HttpMethod);
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
        /// The "GroupName" for documentation lookup.
        /// </summary>
        public String GroupName
        {
            get
            {
                return Utils.GetControllerName(controllerType);
            }
        }

        /// <summary>
        /// The uri template, can be used for "RelativePath" for documentation lookup.
        /// </summary>
        public String UriTemplate
        {
            get
            {
                return halRefInfo.UrlTemplate;
            }
        }

        /// <summary>
        /// Set to true (default) to include docs for this link. Set to false to never include docs for this link.
        /// </summary>
        public bool HasDocs { get; set; } = true;

        /// <summary>
        /// Set this to true to include only docs for the link, with no actual link.
        /// </summary>
        public bool DocsOnly { get; set; } = false;
    }
}
