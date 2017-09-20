using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class RepoConfigGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model);
        }

        private static String Create(String ns, String Model)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Threax.AspNetCore.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using {ns}.Repository;

namespace {ns}.Mappers
{{
    public partial class {Model}RepoConfig : IServiceSetup
    {{
        public void ConfigureServices(IServiceCollection services)
        {{
            OnConfigureServices(services);

            services.TryAddScoped<I{Model}Repository, {Model}Repository>();
        }}

        partial void OnConfigureServices(IServiceCollection services);
    }}
}}";
        }
    }
}
