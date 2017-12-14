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
using Threax.AspNetCore.Models;

namespace Test.InputModels 
{
    public abstract  class ValueInputBase
    {
        public abstract int Num { get; set; }

    }

    [HalModel]
    public partial class ValueInput : ValueInputBase, IValue
    {
    }
}
