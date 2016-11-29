using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalActionLinkAttribute : HalLinkAttribute
    {
        public HalActionLinkAttribute(string rel, Type controllerType, String actionMethod, string title = null, string method = null)
            : base(rel, CreateHref(controllerType, actionMethod, ref method), title, method)
        {
        }

        private static String CreateHref(Type controllerType, String actionMethod, ref String method)
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
