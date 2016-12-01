using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalDocEndpointInfo : IHalDocEndpointInfo
    {
        public HalDocEndpointInfo(Type controllerType)
        {
            this.ControllerType = controllerType;
        }

        public Type ControllerType { get; set; }

        public String ActionMethod { get; set; } = "Get";

        public String GroupArg { get; set; } = "groupName";

        public String MethodArg { get; set; } = "method";

        public String RelativePathArg { get; set; } = "relativePath";

        public String HttpMethod { get; set; }
    }
}
