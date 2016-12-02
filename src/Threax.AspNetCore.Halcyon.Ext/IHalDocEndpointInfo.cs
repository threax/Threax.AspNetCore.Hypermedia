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
    }
}