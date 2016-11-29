using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    internal static class ControllerUtils
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
    }
}
