using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Test.Controllers.Api;

namespace Test.ViewModels
{
    [HalActionLink(typeof(LotsaValuesController), nameof(LotsaValuesController.List), "ListLotsaValues")]
    [HalActionLink(typeof(LotsaValuesController), nameof(LotsaValuesController.Add), "AddValue")]
    public partial class EntryPoint
    {
        
    }
}