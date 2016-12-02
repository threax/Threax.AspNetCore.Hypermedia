using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class EndpointDocFinder : IEndpointDocFinder
    {
        ISchemaFinder schemaFinder;
        IApiDescriptionGroupCollectionProvider descriptionProvider;

        public EndpointDocFinder(ISchemaFinder schemaFinder, IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            this.schemaFinder = schemaFinder;
            this.descriptionProvider = descriptionProvider;
        }

        public EndpointDescription FindDoc(String groupName, String method, String relativePath)
        {
            if (relativePath.EndsWith("/") || relativePath.EndsWith("\\"))
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            var group = descriptionProvider.ApiDescriptionGroups.Items.First(i => i.GroupName == groupName);
            var action = group.Items.First(i => i.HttpMethod == method && i.RelativePath == relativePath);

            var description = new EndpointDescription();
            foreach (var param in action.ParameterDescriptions)
            {
                if (param.Source.IsFromRequest && param.Source.Id == "Body")
                {
                    description.RequestSchema = JObject.Parse(schemaFinder.Find(param.Type));
                }
            }

            var controllerActionDesc = action.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDesc != null)
            {
                var methodInfo = controllerActionDesc.MethodInfo;
                if (methodInfo.ReturnType != typeof(void))
                {
                    description.ResponseSchema = JObject.Parse(schemaFinder.Find(methodInfo.ReturnType));
                }
            }

            return description;
        }
    }
}
