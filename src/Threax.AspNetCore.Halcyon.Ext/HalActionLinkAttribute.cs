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

        public HalActionLinkAttribute(string rel, Type controllerType, String actionMethod, String[] routeArgs = null, string title = null, string method = null)
            : base(rel, CreateHref(controllerType, actionMethod, routeArgs, ref method), title, method)
        {
            this.controllerType = controllerType;
            this.actionMethod = actionMethod;
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

        private static String CreateHref(Type controllerType, String actionMethod, String[] routeArgs, ref String method)
        {
            var template = "";
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

            return template.Replace("[controller]", Utils.GetControllerName(controllerType)).Replace("[action]", actionMethod);
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
