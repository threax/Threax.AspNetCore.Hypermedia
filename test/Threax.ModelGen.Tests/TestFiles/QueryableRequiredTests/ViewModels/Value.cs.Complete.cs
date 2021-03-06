using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Test.ViewModels 
{
    [HalModel]
    [HalSelfActionLink(typeof(ValuesController), nameof(ValuesController.Get))]
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Update))]
    [HalActionLink(typeof(ValuesController), nameof(ValuesController.Delete))]
    public partial class Value : ICreatedModified
    {
        public Guid ValueId { get; set; }

        public String Info { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}
