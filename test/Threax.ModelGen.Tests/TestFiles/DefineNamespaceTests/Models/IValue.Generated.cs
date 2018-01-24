using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using An.Extra.Namespace;
using Can.Be.Multiline;

namespace Test.Models 
{
    public partial interface IValue 
    {
    }

    public partial interface IValueId
    {
        Guid ValueId { get; set; }
    }    

    public partial interface IValueQuery
    {
        Guid? ValueId { get; set; }

    }
}