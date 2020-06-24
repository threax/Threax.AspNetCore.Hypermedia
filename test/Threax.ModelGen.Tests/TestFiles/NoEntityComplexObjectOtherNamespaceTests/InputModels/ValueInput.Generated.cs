using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.ModelGen.Tests.Models.OtherNamespace;

namespace Test.InputModels 
{
    [HalModel]
    public partial class ValueInput
    {
        public OtherNamespaceClass Info { get; set; }

    }
}
