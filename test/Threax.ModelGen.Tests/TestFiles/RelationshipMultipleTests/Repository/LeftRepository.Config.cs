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
    public partial class LeftRepositoryConfig : IServiceSetup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            OnConfigureServices(services);

            services.TryAddScoped<ILeftRepository, LeftRepository>();
        }

        partial void OnConfigureServices(IServiceCollection services);
    }
}