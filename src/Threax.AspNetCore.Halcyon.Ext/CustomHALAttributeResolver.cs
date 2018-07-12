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
    public class CustomHALAttributeResolver
    {
        public static IHALModelConfig GetConfig(object model)
        {
            var type = model.GetType();

            // is it worth caching this?
            var classAttributes = type.GetTypeInfo().GetCustomAttributes();

            foreach (var attribute in classAttributes)
            {
                var modelAttribute = attribute as HalModelAttribute;
                if (modelAttribute != null)
                {
                    if (modelAttribute.ForceHal.HasValue || modelAttribute.LinkBase != null)
                    {
                        var config = new HALModelConfig();
                        if (modelAttribute.ForceHal.HasValue)
                        {
                            config.ForceHAL = modelAttribute.ForceHal.Value;
                        }
                        if (modelAttribute.LinkBase != null)
                        {
                            config.LinkBase = modelAttribute.LinkBase;
                        }
                        return config;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get the links that the current user is allowed to use.
        /// </summary>
        /// <param name="model">The model class to scan.</param>
        /// <param name="context">The curren httpcontext.</param>
        /// <param name="endpointInfo">The info for this endpoint.</param>
        /// <returns></returns>
        public static IEnumerable<Link> GetUserLinks(object model, HttpContext context, IHalDocEndpointInfo endpointInfo)
        {
            var type = model.GetType();
            var classAttributes = type.GetTypeInfo().GetCustomAttributes();

            var queryProvider = model as IQueryStringProvider;

            foreach (var attribute in classAttributes)
            {
                //Handle our HalLinkAttributes, this way we can make sure the user can access the links
                var actionLinkAttribute = attribute as HalActionLinkAttribute;
                if (actionLinkAttribute != null)
                {
                    if (actionLinkAttribute.CanUserAccess(context.User))
                    {
                        if (!actionLinkAttribute.DocsOnly)
                        {
                            var href = actionLinkAttribute.Href;
                            Object requestData = null;
                            if (queryProvider != null)
                            {
                                var builder = new RequestDataBuilder();
                                queryProvider.AddQuery(actionLinkAttribute.Rel, builder);
                                requestData = builder.Data;
                            }
                            yield return new Link(actionLinkAttribute.Rel, href, actionLinkAttribute.Title, actionLinkAttribute.Method, dataMode: actionLinkAttribute.DataMode, requestData: requestData);
                        }

                        if (endpointInfo != null && actionLinkAttribute.HasDocs)
                        {
                            var docLink = actionLinkAttribute.GetDocLink(endpointInfo);
                            if (docLink != null)
                            {
                                yield return new Link(docLink.Rel, docLink.Href, docLink.Title, docLink.Method, false, dataMode: docLink.DataMode, requestData: docLink.RequestData); //Don't replace parameters for these, already done
                            }
                        }
                    }
                }
                else
                {
                    var linkAttribute = attribute as HalLinkAttribute;
                    if (linkAttribute != null)
                    {
                        yield return new Link(linkAttribute.Rel, linkAttribute.Href, linkAttribute.Title, linkAttribute.Method, dataMode: linkAttribute.DataMode, requestData: linkAttribute.RequestData);
                    }
                }
            }

            var linkProvider = model as IHalLinkProvider;
            if (linkProvider != null)
            {
                var providerContext = new LinkProviderContext(context);
                var links = linkProvider.CreateHalLinks(providerContext);
                foreach (var linkAttribute in links)
                {
                    //If we are getting action link attributes, do additional customization.
                    var actionLinkAttribute = linkAttribute as HalActionLinkAttribute;
                    if (actionLinkAttribute != null)
                    {
                        if (actionLinkAttribute.CanUserAccess(context.User))
                        {
                            yield return new Link(linkAttribute.Rel, linkAttribute.Href, linkAttribute.Title, linkAttribute.Method, dataMode: linkAttribute.DataMode, requestData: linkAttribute.RequestData);

                            //Include doc links for link provider links
                            if (endpointInfo != null && actionLinkAttribute.HasDocs)
                            {
                                var docLink = actionLinkAttribute.GetDocLink(endpointInfo);
                                if (docLink != null)
                                {
                                    yield return new Link(docLink.Rel, docLink.Href, docLink.Title, docLink.Method, false, dataMode: docLink.DataMode, requestData: docLink.RequestData); //Don't replace parameters for these, already done
                                }
                            }
                        }
                    }
                    else //Otherwise just include the link
                    {
                        yield return new Link(linkAttribute.Rel, linkAttribute.Href, linkAttribute.Title, linkAttribute.Method, dataMode: linkAttribute.DataMode, requestData: linkAttribute.RequestData);
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, IEnumerable>> GetEmbeddedCollectionValues(object model)
        {
            var type = model.GetType();
            var embeddedModelProperties = type.GetTypeInfo().GetProperties().Where(x => x.IsDefined(typeof(HalEmbeddedAttribute), true));

            foreach (var propertyInfo in embeddedModelProperties)
            {
                var embeddAttribute = propertyInfo.GetCustomAttribute(typeof(HalEmbeddedAttribute)) as HalEmbeddedAttribute;
                if (embeddAttribute == null) continue;

                var modelValue = propertyInfo.GetValue(model) as IEnumerable;
                if (modelValue != null)
                {
                    yield return new KeyValuePair<string, IEnumerable>(embeddAttribute.CollectionName, modelValue);
                }
            }
        }
    }
}
