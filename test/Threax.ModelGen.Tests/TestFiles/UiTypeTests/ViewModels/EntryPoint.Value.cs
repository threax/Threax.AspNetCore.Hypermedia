using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Test.Controllers.Api;

namespace Test.ViewModels
{
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.List), "ListValues")]
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Add), "AddValue")]
    public partial class EntryPoint
    {
        
    }
}