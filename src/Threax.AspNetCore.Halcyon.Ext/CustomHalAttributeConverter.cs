using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class CustomHalAttributeConverter : IHALConverter
    {
        private HttpContext httpContext;
        private HalcyonConventionOptions options;

        public CustomHalAttributeConverter(IHttpContextAccessor httpContextAccessor, HalcyonConventionOptions options)
        {
            this.httpContext = httpContextAccessor.HttpContext;
            this.options = options;
        }

        public bool CanConvert(Type type)
        {
            if (type == null || type == typeof(HALResponse))
            {
                return false;
            }

            return type.GetTypeInfo().GetCustomAttributes().Any(x => x is HalModelAttribute);
        }

        public HALResponse Convert(object model)
        {
            if (!this.CanConvert(model?.GetType()))
            {
                throw new InvalidOperationException($"Cannot convert type {model.GetType().FullName}. Did you forget a HalModelAttribute on the model class you are returning?");
            }

            //If the object is an ICollectionView, use that to parse.
            var dataCollection = model as ICollectionView;
            if(dataCollection != null)
            {
                var itemType = dataCollection.CollectionType;
                var response = ConvertInstance(model, httpContext, options);
                response.AddEmbeddedCollection("values", GetEmbeddedResponses(dataCollection.AsObjects, httpContext, options));
                return response;
            }

            //If the object is an IEnumerable try to identify properties from that.
            var enumerableValue = model as IEnumerable;
            if (enumerableValue != null)
            {
                var itemType = Utils.GetEnumerableModelType(enumerableValue);
                var response = new HALResponse(new Object());
                response.AddEmbeddedCollection("values", GetEmbeddedResponses(enumerableValue, httpContext, options));
                return response;
            }

            //If we got here we probably have a plain object, convert and return it.
            return ConvertInstance(model, httpContext, options);
        }

        private static HALResponse ConvertInstance(object model, HttpContext context, HalcyonConventionOptions options)
        {
            //If this is called for a collection it will scan all the links for each item, but
            //each one needs to be customized to work anyway.

            var resolver = new CustomHALAttributeResolver();

            //If the options provide a model, use that, otherwise get it from the resolver.
            IHALModelConfig halConfig;
            if(options.BaseUrl != null)
            {
                var pathBaseValue = "";
                var pathBase = context.Request.PathBase;
                if (pathBase.HasValue)
                {
                    //If we have a value, use that as the pathBaseValue, otherwise stick with the empty string.
                    pathBaseValue = pathBase.Value;
                }

                var currentUri = new Uri(context.Request.GetDisplayUrl());
                var host = $"{currentUri.Scheme}://{currentUri.Authority}{pathBaseValue}";

                halConfig = new HALModelConfig()
                {
                    LinkBase = options.BaseUrl.Replace(HalcyonConventionOptions.HostVariable, host),
                    ForceHAL = false
                };
            }
            else
            {
                halConfig = resolver.GetConfig(model);
            }

            var response = new HALResponse(model, halConfig);
            response.AddLinks(resolver.GetUserLinks(model, context, options.HalDocEndpointInfo));
            response.AddEmbeddedCollections(resolver.GetEmbeddedCollections(model, halConfig));

            return response;
        }

        private static IEnumerable<HALResponse> GetEmbeddedResponses(IEnumerable enumerableValue, HttpContext context, HalcyonConventionOptions options)
        {
            foreach (var item in enumerableValue)
            {
                yield return ConvertInstance(item, context, options);
            }
        }
    }
}
