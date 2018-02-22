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
    public partial interface IRight 
    {
        String Info { get; set; }

    }

    public partial interface IRightId
    {
        Guid RightId { get; set; }
    }    

    public partial interface IRightQuery
    {
        Guid? RightId { get; set; }

    }
}