using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Test.InputModels 
{
    [HalModel]
    public partial class LeftInput
    {
        public String Info { get; set; }

        [ValueProvider(typeof(Threax.ModelGen.Tests.Models.RelationshipNatural.WithValueProvider.RightValueProvider))]
        public List<Guid> RightIds { get; set; }

    }
}
