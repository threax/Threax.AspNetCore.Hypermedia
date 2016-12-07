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
    public class HalSelfActionLinkAttribute : HalActionLinkAttribute
    {
        /// <summary>
        /// Create a new link based on a controller and a function.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="controllerType"></param>
        /// <param name="actionMethod"></param>
        /// <param name="routeArgs"></param>
        /// <param name="title"></param>
        /// <param name="method"></param>
        /// <param name="templateDontProvide">This is used to hold some data during construction, no need to provide this param as it is always overwritten.</param>
        public HalSelfActionLinkAttribute(string rel, Type controllerType, String[] routeArgs = null, string title = null)
            :base("self", rel, controllerType, routeArgs, title)
        {
            
        }
    }
}
