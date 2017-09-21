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
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HalSelfActionLinkAttribute : HalActionLinkAttribute
    {
        public const String SelfRelName = "self";

        /// <summary>
        /// Create a new link based on a controller and a function.
        /// These links will include thier request query and have no docs
        /// by default. They should also only be placed on get rels.
        /// </summary>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="funcName">The name of the function on the controller to lookup.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">The title.</param>
        public HalSelfActionLinkAttribute(Type controllerType, String funcName, String[] routeArgs = null, string title = null)
            : base(SelfRelName, controllerType, funcName, routeArgs, title)
        {
            HasDocs = false;
        }

        /// <summary>
        /// Create a new link based on a controller and a function.
        /// These links will include thier request query and have no docs
        /// by default. They should also only be placed on get rels.
        /// </summary>
        /// <param name="rel">The name of the rel to lookup.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="routeArgs">Any additional route args.</param>
        /// <param name="title">The title.</param>
        public HalSelfActionLinkAttribute(string rel, Type controllerType, String[] routeArgs = null, string title = null)
            :base(SelfRelName, rel, controllerType, routeArgs, title)
        {
            HasDocs = false;
        }
    }
}
