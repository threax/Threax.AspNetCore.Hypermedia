using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalcyonClientSetup<T> : IHalcyonClientSetup<T>
    {
        public HalcyonClientSetup(IServiceCollection services)
        {
            this.Services = services;
        }

        public IServiceCollection Services { get; private set; }
    }
}
