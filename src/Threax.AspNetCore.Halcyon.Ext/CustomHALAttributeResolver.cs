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
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<Link> GetUserLinks(object model, HttpContext context, IHalDocEndpointInfo endpointInfo)
        {
            var type = model.GetType();
            var classAttributes = type.GetTypeInfo().GetCustomAttributes();

            foreach (var attribute in classAttributes)
            {
                //Handle our HalLinkAttributes, this way we can make sure the user can access the links
                var actionLinkAttribute = attribute as HalActionLinkAttribute;
                if (actionLinkAttribute != null)
                {
                    if (actionLinkAttribute.CanUserAccess(context.User))
                    {
                        var href = actionLinkAttribute.Href;
                        if (actionLinkAttribute.IncludeRequestQuery)
                        {
                            var displayUrlBuilder = new UriBuilder(context.Request.GetDisplayUrl());
                            var query = displayUrlBuilder.Query;
                            if (!String.IsNullOrEmpty(query))
                            {
                                if (href.Contains('?'))
                                {
                                    href += "&" + displayUrlBuilder.Query.Substring(1);
                                }
                                else
                                {
                                    href += displayUrlBuilder.Query;
                                }
                            }
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
