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
    /// <summary>
    /// This attribute adds a self link to the result, it will use the current url from the request
    /// to generate this.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HalSelfLinkAttribute : Attribute
    {
        public HalSelfLinkAttribute()
        {
            
        }
    }
}
