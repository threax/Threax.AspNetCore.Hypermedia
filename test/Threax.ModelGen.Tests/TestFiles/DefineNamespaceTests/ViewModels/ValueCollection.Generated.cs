using Halcyon.HAL.Attributes;
using Test.Controllers.Api;
using Test.Models;
using Test.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using An.Extra.Namespace;
using Can.Be.Multiline;

namespace Test.ViewModels
{
    public partial class ValueCollection : PagedCollectionViewWithQuery<Value, ValueQuery>
    {
        
    }
}