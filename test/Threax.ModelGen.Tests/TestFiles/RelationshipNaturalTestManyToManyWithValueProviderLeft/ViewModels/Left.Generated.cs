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
    public partial class Left : ICreatedModified
    {
        public Guid LeftId { get; set; }

        public String Info { get; set; }

        [ValueProvider(typeof(Threax.ModelGen.Tests.Models.RelationshipNatural.WithValueProvider.RightValueProvider))]
        public List<Guid> RightIds { get; set; }

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}
