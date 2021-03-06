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
    public partial class Right : ICreatedModified
    {
        public Guid RightId { get; set; }

        public String Info { get; set; }

        public List<Guid> LeftIds { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}
