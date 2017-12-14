using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Test.Models;
using Test.Controllers.Api;

namespace Test.ViewModels
{
    [HalModel]
    [HalSelfActionLink(typeof(LotsaValuesController), nameof(LotsaValuesController.Get))]
    [HalActionLink(typeof(LotsaValuesController), nameof(LotsaValuesController.Update))]
    [HalActionLink(typeof(LotsaValuesController), nameof(LotsaValuesController.Delete))]
    public partial class Value
    {
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See Value.Generated for the generated code
    }
}