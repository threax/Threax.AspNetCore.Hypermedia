using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    }
}
