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
        private String actionMethod;
        private String relativePath;

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
        public HalActionLinkAttribute(string rel, Type controllerType, String[] routeArgs = null, string title = null, string method = null, string templateDontProvide = null, string actionMethodDontProvide = null)
            : this(rel, rel, controllerType, routeArgs, title, method, templateDontProvide, actionMethodDontProvide)
        {
            //Everything done in other constructor
        }

        /// <summary>
        /// This constructor allows us to differentiate the real rel we pass to the base class from the one we lookup, useful for self links.
        /// </summary>
        protected HalActionLinkAttribute(string realRel, string lookupRel, Type controllerType, String[] routeArgs = null, string title = null, string method = null, string templateDontProvide = null, string actionMethodDontProvide = null)
            : base(realRel, CreateHref(lookupRel, controllerType, routeArgs, out method, out templateDontProvide, out actionMethodDontProvide), title, method)
        {
            this.controllerType = controllerType;
            this.relativePath = templateDontProvide;
            this.actionMethod = actionMethodDontProvide;
        }

        public bool CanUserAccess(ClaimsPrincipal claims)
        {
            bool canVisit = true;
            var methodInfo = controllerType.GetTypeInfo().GetMethod(actionMethod);
            if (methodInfo != null)
            {
                canVisit = Utils.CheckRoles(claims, methodInfo.GetCustomAttributes<AuthorizeAttribute>(true));
            }
            return canVisit;
        }

        public HalLinkAttribute GetDocLink(IHalDocEndpointInfo docEndpointInfo)
        {
            var methodInfo = controllerType.GetTypeInfo().GetMethod(this.actionMethod);
            if (methodInfo == null)
            {
                return null;
            }

            //Create a link to the endpoint info for this controller and action method.
            String method, template, actionMethod;
            return new HalLinkAttribute($"{this.Rel}.Docs", CreateHref(docEndpointInfo.Rel, docEndpointInfo.ControllerType,
                new String[] {
                    $"{docEndpointInfo.GroupArg}={Utils.GetControllerName(controllerType)}",
                    $"{docEndpointInfo.MethodArg}={Method}",
                    $"{docEndpointInfo.RelativePathArg}={relativePath}"
                }, out method, out template, out actionMethod), null, method);
        }

        protected static String CreateHref(String rel, Type controllerType, String[] routeArgs, out String method, out String template, out string actionMethod)
        {
            method = "GET"; //Get by default
            template = "";
            var controllerTypeInfo = controllerType.GetTypeInfo();
            //Look at the controller
            var routeAttr = controllerTypeInfo.GetCustomAttribute<RouteAttribute>();
            if (routeAttr != null)
            {
                template += routeAttr.Template;
            }

            MethodInfo methodInfo = null;
            foreach(var item in controllerTypeInfo.DeclaredMethods.Concat(controllerTypeInfo.GetMethods(BindingFlags.Public)))
            {
                //This loop will search the DeclaredMethods first since we are most likely to find the method there, then all of them
                var relAttr = item.GetCustomAttribute<HalRelAttribute>();
                if (relAttr != null && relAttr.Rel == rel)
                {
                    methodInfo = item;
                    break;
                }
            }

            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Cannot find an action method with the rel {rel} on {controllerType.Name}. Do you need to define a HalRel attribute on your target method?");
            }

            actionMethod = methodInfo.Name;

            var methodAttribute = methodInfo.GetCustomAttribute<HttpMethodAttribute>();
            if (methodAttribute != null)
            {
                var trailingChar = template[template.Length - 1];
                if (template.Length > 0 && trailingChar != '/' && trailingChar != '\\')
                {
                    template += '/';
                }

                template += methodAttribute.Template;
                method = methodAttribute.HttpMethods.FirstOrDefault();
            }

            if(template.Length == 0)
            {
                throw new InvalidOperationException($"Cannot build a route template for rel {rel} in controller {controllerType}. Did you forget to add a HttpMethod (HttpGet, HttpPost etc) attribute to the Action Method or a RouteAttribute to the controller class.");
            }

            //Remove the * from any route variables that include one
            template = template.Replace("{*", "{").Replace("[controller]", Utils.GetControllerName(controllerType)).Replace("[action]", actionMethod);

            if(routeArgs != null)
            {
                foreach(var arg in routeArgs)
                {
                    int firstEquals = arg.IndexOf('=');
                    if(firstEquals != -1)
                    {
                        var key = $"{{{arg.Substring(0, firstEquals)}}}";
                        var value = arg.Substring(firstEquals + 1).Trim();
                        template = template.Replace(key, value);
                    }
                    else
                    {
                        throw new InvalidOperationException($"The route argument {arg} for Action Method {actionMethod} in controller {controllerType} is not valid. It must be in the format key=value.");
                    }
                }
            }

            return template;
        }

        public static String GetFunctionName<T, DelegateType>(Expression<Func<T, DelegateType>> expr)
        {
            UnaryExpression unaryExpr = (UnaryExpression)expr.Body;
            MethodCallExpression methodCallExpr = (MethodCallExpression)unaryExpr.Operand;
            ConstantExpression constantExpr = (ConstantExpression)methodCallExpr.Arguments[2];
            MethodInfo methodInfo = (MethodInfo)constantExpr.Value;
            return methodInfo.Name;
        }
    }
}
