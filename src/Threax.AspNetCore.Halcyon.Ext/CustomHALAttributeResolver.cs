using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class CustomHALAttributeResolver : HALAttributeResolver
    {
        /// <summary>
        /// Get the links that the current user is allowed to use.
        /// </summary>
        /// <param name="model">The model class to scan.</param>
        /// <param name="context">The curren httpcontext.</param>
        /// <param name="endpointInfo">The info for this endpoint.</param>
        /// <returns></returns>
        public IEnumerable<Link> GetUserLinks(object model, HttpContext context, IHalDocEndpointInfo endpointInfo)
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
                        var href = actionLinkAttribute.Href;
                        if(queryProvider != null)
                        {
                            var builder = new QueryStringBuilder();
                            queryProvider.AddQuery(actionLinkAttribute.Rel, builder);
                            href = builder.AddToUrl(href);
                        }
                        yield return new Link(actionLinkAttribute.Rel, href, actionLinkAttribute.Title, actionLinkAttribute.Method);

                        if (endpointInfo != null && actionLinkAttribute.HasDocs)
                        {
                            var docLink = actionLinkAttribute.GetDocLink(endpointInfo);
                            if (docLink != null)
                            {
                                yield return new Link(docLink.Rel, docLink.Href, docLink.Title, docLink.Method, false); //Don't replace parameters for these, already done
                            }
                        }
                    }
                }
                else
                {
                    var linkAttribute = attribute as HalLinkAttribute;
                    if (linkAttribute != null)
                    {
                        yield return new Link(linkAttribute.Rel, linkAttribute.Href, linkAttribute.Title, linkAttribute.Method);
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
                    yield return new Link(linkAttribute.Rel, linkAttribute.Href, linkAttribute.Title, linkAttribute.Method);
                }
            }
        }
    }
}
