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
    [HalSelfActionLink(typeof(LeftsController), nameof(LeftsController.Get))]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Update))]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Delete))]
    public partial class Left
    {
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See Left.Generated for the generated code
    }
}