﻿using Microsoft.AspNetCore.Http;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ILinkProviderContext
    {
        HttpContext HttpContext { get; set; }
    }
}