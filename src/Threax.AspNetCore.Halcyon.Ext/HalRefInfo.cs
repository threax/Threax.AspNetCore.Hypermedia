using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This class is used by HalActionLinkAttribute to lookup info about the action link provided.
    /// </summary>
    public class HalRefInfo
    {
        public HalRefInfo(String rel, Type controllerType, String[] routeArgs)
        {
            var controllerTypeInfo = controllerType.GetTypeInfo();
            //Look at the controller
            var routeAttr = controllerTypeInfo.GetCustomAttribute<RouteAttribute>();
            if (routeAttr != null)
            {
                this.UrlTemplate += routeAttr.Template;
            }

            foreach (var item in controllerTypeInfo.DeclaredMethods.Concat(controllerTypeInfo.GetMethods(BindingFlags.Public)))
            {
                //This loop will search the DeclaredMethods first since we are most likely to find the method there, then all of them
                var relAttr = item.GetCustomAttribute<HalRelAttribute>();
                if (relAttr != null && relAttr.Rel == rel)
                {
                    this.ActionMethodInfo = item;
                    break;
                }
            }

            if (this.ActionMethodInfo == null)
            {
                throw new InvalidOperationException($"Cannot find an action method with the rel {rel} on {controllerType.Name}. Do you need to define a HalRel attribute on your target method?");
            }

            var actionMethodName = this.ActionMethodInfo.Name;

            var methodAttribute = this.ActionMethodInfo.GetCustomAttribute<HttpMethodAttribute>();
            if (methodAttribute != null)
            {
                var trailingChar = this.UrlTemplate[this.UrlTemplate.Length - 1];
                if (this.UrlTemplate.Length > 0 && trailingChar != '/' && trailingChar != '\\')
                {
                    this.UrlTemplate += '/';
                }

                this.UrlTemplate += methodAttribute.Template;
                this.HttpMethod = methodAttribute.HttpMethods.FirstOrDefault();
            }

            if (this.UrlTemplate.Length == 0)
            {
                throw new InvalidOperationException($"Cannot build a route template for rel {rel} in controller {controllerType}. Did you forget to add a HttpMethod (HttpGet, HttpPost etc) attribute to the Action Method or a RouteAttribute to the controller class.");
            }

            //Remove the * from any route variables that include one
            this.UrlTemplate = this.UrlTemplate.Replace("{*", "{").Replace("[controller]", Utils.GetControllerName(controllerType)).Replace("[action]", actionMethodName);

            if (routeArgs != null)
            {
                foreach (var arg in routeArgs)
                {
                    int firstEquals = arg.IndexOf('=');
                    if (firstEquals != -1)
                    {
                        var key = $"{{{arg.Substring(0, firstEquals)}}}";
                        var value = arg.Substring(firstEquals + 1).Trim();
                        this.UrlTemplate = this.UrlTemplate.Replace(key, value);
                    }
                    else
                    {
                        throw new InvalidOperationException($"The route argument {arg} for Action Method {actionMethodName} in controller {controllerType} is not valid. It must be in the format key=value.");
                    }
                }
            }
        }

        public string HttpMethod { get; internal set; } = "GET";

        public string UrlTemplate { get; internal set; } = "";

        /// <summary>
        /// The discovered method info, prevents a second lookup later in the request.
        /// </summary>
        public MethodInfo ActionMethodInfo { get; set; }
    }
}
