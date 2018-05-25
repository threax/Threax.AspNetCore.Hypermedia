﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public interface IClientGenerator
    {
        Task<IEnumerable<EndpointClientDefinition>> GetEndpointDefinitions();
    }
}
