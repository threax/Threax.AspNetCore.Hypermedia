using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Test.Controllers.Api;

namespace Test.ViewModels
{
    [HalActionLink(typeof(RightsController), nameof(RightsController.List), "ListRights")]
    [HalActionLink(typeof(RightsController), nameof(RightsController.Add), "AddRight")]
    public partial class EntryPoint
    {
        
    }
}