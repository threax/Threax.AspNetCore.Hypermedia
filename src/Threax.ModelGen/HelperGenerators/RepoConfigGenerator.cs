using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public static class RepoConfigGenerator
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"Repository/{schema.Title}Repository.Config.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
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
using Threax.ReflectedServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using {ns}.Repository;

namespace {ns}.Repository.Config
{{
    public partial class {Model}RepositoryConfig : IServiceSetup
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
