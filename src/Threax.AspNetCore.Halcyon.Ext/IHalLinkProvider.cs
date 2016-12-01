﻿using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// By implemething this interface in your model you can customize the links provided.
    /// </summary>
    public interface IHalLinkProvider
    {
        IEnumerable<HalLinkAttribute> CreateHalLinks(ILinkProviderContext context);
    }
}
