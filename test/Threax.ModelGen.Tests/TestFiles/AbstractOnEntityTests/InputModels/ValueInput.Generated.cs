using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Test.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Test.InputModels 
{
    [HalModel]
    public partial class ValueInput : IValue
    {
        public int Num { get; set; }

    }
}
