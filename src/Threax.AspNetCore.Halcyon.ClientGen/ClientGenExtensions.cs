﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public static class ClientGenExtensions
    {
        public static IServiceCollection AddHalClientGen(this IServiceCollection services, HalClientGenOptions options)
        {
            services.AddScoped<IResultViewProvider>(s => new DefaultResultViewProvider(options.SourceAssemblies));
            services.AddScoped<IClientGenerator, ClientGenerator>();
            services.AddScoped<TypescriptClientWriter>();

            return services;
        }
    }
}
