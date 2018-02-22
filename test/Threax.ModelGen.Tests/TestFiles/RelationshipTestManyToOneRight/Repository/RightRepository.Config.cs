using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Threax.AspNetCore.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Test.Repository;

namespace Test.Repository.Config
{
    public partial class RightRepositoryConfig : IServiceSetup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            OnConfigureServices(services);

            services.TryAddScoped<IRightRepository, RightRepository>();
        }

        partial void OnConfigureServices(IServiceCollection services);
    }
}