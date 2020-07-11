using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalDocEndpointInfo : IHalDocEndpointInfo
    {
        public static class DefaultRels
        {
            public const String Get = "get";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="controllerType">The type of the controller that creates endpoint docs.</param>
        public HalDocEndpointInfo(Type controllerType)
            :this(controllerType, null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="controllerType">The type of the controller that creates endpoint docs.</param>
        /// <param name="version">A version to use for the version route arg of the endpoint doc controller. This can be passed as a plain string it will be url encoded by this constructor. If this is null no version will be supplied to the EndpointDocController.</param>
        public HalDocEndpointInfo(Type controllerType, String version)
        {
            this.ControllerType = controllerType;
            this.Version = version != null ? WebUtility.UrlEncode(version) : null;
        }

        public Type ControllerType { get; private set; }

        public String Version { get; private set; }

        public String Rel { get; set; } = DefaultRels.Get;

        public String GroupArg { get; set; } = "groupName";

        public String MethodArg { get; set; } = "method";

        public String RelativePathArg { get; set; } = "relativePath";

        public string VersionArg { get; set; } = "version";
    }
}
