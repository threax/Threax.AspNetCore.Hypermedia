using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalDocEndpointInfo : IHalDocEndpointInfo
    {
        public static class DefaultRels
        {
            public const String Get = "get";
        }

        public HalDocEndpointInfo(Type controllerType)
        {
            this.ControllerType = controllerType;
        }

        public Type ControllerType { get; set; }

        public String Rel { get; set; } = DefaultRels.Get;

        public String GroupArg { get; set; } = "groupName";

        public String MethodArg { get; set; } = "method";

        public String RelativePathArg { get; set; } = "relativePath";

        public String HttpMethod { get; set; }
    }
}
