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
    public partial class ValueInput
    {
        [MaxLength(350, ErrorMessage = "Info must be less than 350 characters.")]
        public String Info { get; set; }

    }
}
