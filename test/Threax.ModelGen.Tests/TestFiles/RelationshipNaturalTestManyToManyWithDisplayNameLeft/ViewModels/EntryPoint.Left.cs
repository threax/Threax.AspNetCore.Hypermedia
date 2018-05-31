using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Test.Controllers.Api;

namespace Test.ViewModels
{
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.List), "ListLefts")]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Add), "AddLeft")]
    public partial class EntryPoint
    {
        
    }
}