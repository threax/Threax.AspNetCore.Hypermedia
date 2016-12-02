using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Halcyon.Web.HAL.Json;
using Threax.AspNetCore.Halcyon.Ext;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Halcyon.HAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions class for IServiceCollection for the Halcyon convention.
    /// </summary>
    public static class HalcyonConvention
    {
        /// <summary>
        /// Add the conventional Halcyon services. This will set everything up for attributes to work correctly.
        /// You must use this function when calling UseConventionalHalcyon on MvcOptions or the services will
        /// not be setup correctly.
        /// </summary>
        /// <param name="services">The service collection to modify.</param>
        /// <returns></returns>
        public static IServiceCollection AddConventionalHalcyon(this IServiceCollection services, HalcyonConventionOptions options = null)
        {
            if(options == null)
            {
                options = new HalcyonConventionOptions();
            }

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.TryAddSingleton<HalcyonConventionOptions>(options);
            services.TryAddScoped<IHALConverter, CustomHalAttributeConverter>();
            services.TryAddScoped<HalModelResultFilterAttribute, HalModelResultFilterAttribute>();
            services.TryAddScoped<IHalModelViewMapper, NoViewMapper>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        /// <summary>
        /// Add Halcyon with some custom formatters that handle marked up models better.
        /// </summary>
        /// <param name="mvcOptions">The MVC options to extend.</param>
        /// <returns></returns>
        public static MvcOptions UseConventionalHalcyon(this MvcOptions mvcOptions, JsonSerializerSettings serializerSettings = null)
        {
            mvcOptions.Filters.AddService(typeof(HalModelResultFilterAttribute));

            JsonHalOutputFormatter outputFormatter;
            var mediaTypes = new string[] { "application/json" };
            if (serializerSettings == null)
            {
                outputFormatter = new JsonHalOutputFormatter(mediaTypes);
            }
            else
            {
                outputFormatter = new JsonHalOutputFormatter(serializerSettings, mediaTypes);
            }
            mvcOptions.OutputFormatters.Add(outputFormatter);
            mvcOptions.Filters.Add(new ProducesAttribute("application/json"));

            return mvcOptions;
        }
    }
}
