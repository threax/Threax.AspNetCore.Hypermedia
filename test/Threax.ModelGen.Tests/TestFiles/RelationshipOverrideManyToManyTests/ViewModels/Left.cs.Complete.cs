using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.Models;
using Test.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Test.ViewModels 
{
    [HalModel]
    [HalSelfActionLink(typeof(LeftsController), nameof(LeftsController.Get))]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Update))]
    [HalActionLink(typeof(LeftsController), nameof(LeftsController.Delete))]
    public partial class Left : ICreatedModified
    {
        public Guid LeftId { get; set; }

        public String Info { get; set; }

        [Display(Name = "Cool Right Things")]
        public List<Guid> RightIds { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}
