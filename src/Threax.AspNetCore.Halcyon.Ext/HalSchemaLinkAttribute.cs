using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalSchemaLinkAttribute : HalActionLinkAttribute
    {
        public HalSchemaLinkAttribute(string rel, Type controllerType, String actionMethod, Type schemaType, string title = null, string method = null)
            :base(rel, controllerType, actionMethod, new String[] { $"schema={schemaType.FullName}" }, title, method)
        {

        }
    }
}
