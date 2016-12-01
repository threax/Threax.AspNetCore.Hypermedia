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
        public HalActionLinkAttribute(string rel, Type controllerType, String actionMethod, String[] routeArgs = null, string title = null, string method = null, string templateDontProvide = null)
            : base(rel, CreateHref(controllerType, actionMethod, routeArgs, ref method, ref templateDontProvide), title, method)
        {
            this.controllerType = controllerType;
            this.actionMethod = actionMethod;
            this.relativePath = templateDontProvide;
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
            var methodInfo = controllerType.GetTypeInfo().GetMethod(actionMethod);
            if (methodInfo == null)
            {
                return null;
            }

            //Create a link to the endpoint info for this controller and action method.
            String method = null, template = null;
            return new HalLinkAttribute($"{this.Rel}.Docs", CreateHref(docEndpointInfo.ControllerType, docEndpointInfo.ActionMethod,
                new String[] {
                    $"{docEndpointInfo.GroupArg}={Utils.GetControllerName(controllerType)}",
                    $"{docEndpointInfo.MethodArg}={Method}",
                    $"{docEndpointInfo.RelativePathArg}={relativePath}"
                }, ref method, ref template), null, method);
        }

        private static String CreateHref(Type controllerType, String actionMethod, String[] routeArgs, ref String method, ref String template)
        {
            template = "";
            var controllerTypeInfo = controllerType.GetTypeInfo();
            //Look at the controller
            var routeAttr = controllerTypeInfo.GetCustomAttribute<RouteAttribute>();
            if (routeAttr != null)
            {
                template += routeAttr.Template;
            }

            var methodInfo = controllerTypeInfo.GetMethod(actionMethod);
            if (methodInfo != null)
            {
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
            }

            if(template.Length == 0)
            {
                throw new InvalidOperationException($"Cannot build a route template for Action Method {actionMethod} in controller {controllerType}. Did you forget to add a HttpMethodAttribute (HttpGet, HttpPost etc) to the Action Method or a RouteAttribute to the controller class.");
            }

            //Remove the * from any route variables that include one
            template = template.Replace("{*", "{").Replace("[controller]", Utils.GetControllerName(controllerType)).Replace("[action]", actionMethod); ;

            if(routeArgs != null)
            {
                foreach(var arg in routeArgs)
                {
                    var keyValue = arg.Split('=');
                    if(keyValue.Length == 2)
                    {
                        var key = $"{{{keyValue[0].Trim()}}}";
                        var value = keyValue[1].Trim();
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
