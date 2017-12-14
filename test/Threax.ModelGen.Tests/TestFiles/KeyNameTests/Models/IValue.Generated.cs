using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace Test.Models 
{
    public partial interface IValue 
    {
    }

    public partial interface IValueId
    {
        Guid CrazyKey { get; set; }
    }    

    public partial interface IValueQuery
    {
        Guid? CrazyKey { get; set; }

    }
}