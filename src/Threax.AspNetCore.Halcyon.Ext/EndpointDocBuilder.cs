using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocBuilder
    {
        EndpointDoc GetDoc(String groupName, String method, String relativePath);
    }

    public class EndpointDocBuilder : IEndpointDocBuilder
    {
        private IApiDescriptionGroupCollectionProvider descriptionProvider;
        private ISchemaBuilder schemaBuilder;

        public EndpointDocBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider, ISchemaBuilder schemaBuilder)
        {
            this.descriptionProvider = descriptionProvider;
            this.schemaBuilder = schemaBuilder;
        }

        public virtual EndpointDoc GetDoc(String groupName, String method, String relativePath)
        {
            if(relativePath == null)
            {
                relativePath = "";
            }
            else if (relativePath.EndsWith("/") || relativePath.EndsWith("\\"))
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            var group = descriptionProvider.ApiDescriptionGroups.Items.FirstOrDefault(i => i.GroupName == groupName);
            if(group == null)
            {
                throw new InvalidOperationException($"Cannot find an api group for {groupName}. Did you declare a route to that group?");
            }

            var action = group.Items.FirstOrDefault(i => i.HttpMethod == method && i.RelativePath == relativePath);
            if(action == null)
            {
                throw new InvalidOperationException($"Cannot find an api action for {relativePath} and method {method} in api group {groupName}.");
            }

            var description = new EndpointDoc();
            Type queryModelType = null;
            foreach (var param in action.ParameterDescriptions)
            {
                if (param.Source.IsFromRequest)
                {
                    if (param.Source.CanAcceptDataFrom(BindingSource.Body))
                    {
                        description.RequestSchema = schemaBuilder.GetSchema(param.Type, true);
                    }
                    else if (param.Source.CanAcceptDataFrom(BindingSource.Query))
                    {
                        if (queryModelType == null)
                        {
                            queryModelType = param.ModelMetadata.ContainerType;
                        }
                        else if (queryModelType != param.ModelMetadata.ContainerType)
                        {
                            throw new InvalidOperationException($"Cannot build a query parameter for multiple different models for group: {groupName} method: {method} relativePath: {relativePath}");
                        }
                    }
                }
            }

            if (queryModelType != null)
            {
                description.QuerySchema = schemaBuilder.GetSchema(queryModelType, true);
            }

            var controllerActionDesc = action.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDesc != null)
            {
                var methodInfo = controllerActionDesc.MethodInfo;
                if (methodInfo.ReturnType != typeof(void))
                {
                    description.ResponseSchema = schemaBuilder.GetSchema(methodInfo.ReturnType);
                }
            }

            return description;
        }
    }
}
