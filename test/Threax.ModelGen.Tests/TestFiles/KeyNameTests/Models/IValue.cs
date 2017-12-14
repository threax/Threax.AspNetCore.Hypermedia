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
        //Customize main interface here, see Value.Generated for generated code
    }  

    public partial interface ICrazyKey
    {
        //Customize id interface here, see Value.Generated for generated code
    }    

    public partial interface IValueQuery
    {
        //Customize query interface here, see Value.Generated for generated code
    }
}