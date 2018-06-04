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
    [HalSelfActionLink(typeof(RightsController), nameof(RightsController.Get))]
    [HalActionLink(typeof(RightsController), nameof(RightsController.Update))]
    [HalActionLink(typeof(RightsController), nameof(RightsController.Delete))]
    public partial class Right
    {
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See Right.Generated for the generated code
    }
}