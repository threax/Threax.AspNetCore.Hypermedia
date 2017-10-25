using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    internal static class Utils
    {
        public static String GetControllerName(Type controllerType)
        {
            return GetControllerName(controllerType.Name);
        }

        public static String GetControllerName(String controllerType)
        {
            if (controllerType.EndsWith("Controller"))
            {
                controllerType = controllerType.Substring(0, controllerType.Length - 10);
            }
            return controllerType;
        }

        public static Type GetEnumerableModelType(IEnumerable enumerable)
        {
            return GetEnumerableModelType(enumerable.GetType());
        }

        public static Type GetEnumerableModelType(Type modelType, bool throwOnNotFound = true)
        {
            var elementType = modelType.GetElementType();
            if (elementType == null)
            {
                if (modelType.GenericTypeArguments.Length > 1 && typeof(IDictionary).IsAssignableFrom(modelType))
                {
                    elementType = modelType.GenericTypeArguments[1];
                }
                else if (modelType.GenericTypeArguments.Length > 0)
                {
                    elementType = modelType.GenericTypeArguments[0];
                }
            }

            //See if we found a type, if not either throw or use error depending on the option
            if (elementType == null)
            {
                if (throwOnNotFound)
                {
                    throw new InvalidOperationException($"Could not find the type of the elements of the type '{modelType.Name}'. It must be an array, a dictionary or a collection with 1 generic type argument.");
                }
                else
                {
                    elementType = typeof(Object); //Have to give up and use object
                }
            }

            return elementType;
        }

        public static bool CanUserAccess(ClaimsPrincipal principal, MethodInfo methodInfo, TypeInfo controllerTypeInfo)
        {
            //Check for anon access attribute
            var anonAttrs = methodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true);
            if (anonAttrs.Any())
            {
                return true;
            }
            //Otherwise check the action method and controller for authorize attributes.
            var attributes = methodInfo.GetCustomAttributes<AuthorizeAttribute>(true);
            attributes = attributes.Concat(controllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(true));
            return Utils.CheckRoles(principal, attributes);
        }

        public static bool CheckRoles(ClaimsPrincipal user, IEnumerable<AuthorizeAttribute> authorizeAttrs)
        {
            bool allowAccess = true;
            bool authenticated = user.Identity.IsAuthenticated;
            foreach (var auth in authorizeAttrs)
            {
                allowAccess = allowAccess && authenticated;
                if (auth.Roles != null)
                {
                    foreach (var role in auth.Roles.Split(',').Select(i => i.Trim()))
                    {
                        allowAccess = allowAccess && user.IsInRole(role);
                        if (!allowAccess)
                        {
                            break;
                        }
                    }
                }
                if (!allowAccess)
                {
                    break;
                }
            }

            return allowAccess;
        }
    }
}
