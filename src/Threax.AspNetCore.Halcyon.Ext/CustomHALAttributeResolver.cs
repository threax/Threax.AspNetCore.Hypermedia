using Halcyon.HAL;
using Halcyon.HAL.Attributes;
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
        public IEnumerable<Link> GetUserLinks(object model, ClaimsPrincipal claims)
        {
            var type = model.GetType();
            var classAttributes = type.GetTypeInfo().GetCustomAttributes();

            foreach (var attribute in classAttributes)
            {
                //Handle our HalLinkAttributes, this way we can make sure the user can access the links
                var actionLinkAttribute = attribute as HalActionLinkAttribute;
                if (actionLinkAttribute != null)
                {
                    if (actionLinkAttribute.CanUserAccess(claims))
                    {
                        yield return new Link(actionLinkAttribute.Rel, actionLinkAttribute.Href, actionLinkAttribute.Title, actionLinkAttribute.Method);
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
        }
    }
}
