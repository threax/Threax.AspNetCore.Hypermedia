using System;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IHalDocEndpointInfo
    {
        string Rel { get; set; }
        Type ControllerType { get; set; }
        string GroupArg { get; set; }
        string HttpMethod { get; set; }
        string MethodArg { get; set; }
        string RelativePathArg { get; set; }

        /// <summary>
        /// A version to include on the generated doc links. Including this will make it possible to use caching.
        /// If this is null no version string will be included in the doc links. This should be url escaped already
        /// to save time during requests.
        /// </summary>
        String Version { get; set; }
    }
}