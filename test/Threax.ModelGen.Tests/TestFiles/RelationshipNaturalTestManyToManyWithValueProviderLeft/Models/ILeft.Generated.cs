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
    public partial interface ILeft 
    {
        String Info { get; set; }

    }

    public partial interface ILeftId
    {
        Guid LeftId { get; set; }
    }    

    public partial interface ILeftQuery
    {
        Guid? LeftId { get; set; }

    }
}