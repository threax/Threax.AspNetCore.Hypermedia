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

            var group = descriptionProvider.ApiDescriptionGroups.Items.First(i => i.GroupName == groupName);
            var action = group.Items.First(i => i.HttpMethod == method && i.RelativePath == relativePath);

            var description = new EndpointDoc();
            Type queryModelType = null;
            foreach (var param in action.ParameterDescriptions)
            {
                if (param.Source.IsFromRequest)
                {
                    if (param.Source.CanAcceptDataFrom(BindingSource.Body))
                    {
                        description.RequestSchema = schemaBuilder.GetSchema(param.Type);
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
                description.QuerySchema = schemaBuilder.GetSchema(queryModelType);
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
