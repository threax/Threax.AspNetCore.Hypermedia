using System;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IHalDocEndpointInfo
    {
        string Rel { get; set; }

        /// <summary>
        /// The type of the controller for the endpoint docs.
        /// </summary>
        Type ControllerType { get; }

        /// <summary>
        /// The route arg name on the target EndpointDocController for the group.
        /// </summary>
        string GroupArg { get; set; }

        /// <summary>
        /// The route arg name on the target EndpointDocController for the Http method.
        /// </summary>
        string MethodArg { get; set; }

        /// <summary>
        /// The route arg name on the target EndpointDocController for the relative path.
        /// </summary>
        string RelativePathArg { get; set; }

        /// <summary>
        /// The route arg name on the target EndpointDocController for the version.
        /// </summary>
        string VersionArg { get; set; }

        /// <summary>
        /// A version to include on the generated doc links. Including this will make it possible to use caching.
        /// If this is null no version string will be included in the doc links. This value must be provided on
        /// construction and will be expected to be url encoded already when retrieved.
        /// </summary>
        String Version { get; }
    }
}