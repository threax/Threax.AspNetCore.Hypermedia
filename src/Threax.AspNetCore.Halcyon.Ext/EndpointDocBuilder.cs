using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocBuilder
    {
        /// <summary>
        /// Get the EndpointDoc object for the specified endpoint, include a ClaimsPrincipal to throw an
        /// UnauthorizedAccessException if the user cannot access the endpoint.
        /// </summary>
        /// <param name="groupName">The group name</param>
        /// <param name="method">The method name</param>
        /// <param name="relativePath">The relative path</param>
        /// <param name="user">The user to check, can be null.</param>
        /// <returns>The EndpointDoc object for the endpoint.</returns>
        EndpointDoc GetDoc(String groupName, String method, String relativePath, ClaimsPrincipal user = null);
    }

    public class EndpointDocBuilder : IEndpointDocBuilder
    {
        private IApiDescriptionGroupCollectionProvider descriptionProvider;
        private ISchemaBuilder schemaBuilder;
        private IValidSchemaTypeManager validSchemaManager;

        public EndpointDocBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider, ISchemaBuilder schemaBuilder, IValidSchemaTypeManager validSchemaManager)
        {
            this.descriptionProvider = descriptionProvider;
            this.schemaBuilder = schemaBuilder;
            this.validSchemaManager = validSchemaManager;
        }

        public virtual EndpointDoc GetDoc(String groupName, String method, String relativePath, ClaimsPrincipal user = null)
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
            bool handleFormData = true;

            var controllerActionDesc = action.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDesc != null)
            {
                var methodInfo = controllerActionDesc.MethodInfo;

                //Check to see if the user can actually access the endpoint we requested
                if (user != null && !Utils.CanUserAccess(user, methodInfo, controllerActionDesc.ControllerTypeInfo))
                {
                    throw new UnauthorizedAccessException("User cannot access requested endpoint");
                }

                var returnType = methodInfo.ReturnType;
                if (returnType != typeof(void))
                {
                    description.ResponseSchema = schemaBuilder.GetSchema(returnType);
                }
            }

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
                    else if(handleFormData && param.Source.CanAcceptDataFrom(BindingSource.Form))
                    {
                        handleFormData = false; //This prevents this from running for everything that responsds to form, there should only be 1 per controller method.
                        //Discover the type from the action method, there is no way to get the real object type from the description when dealing with form input
                        Type type = null;
                        var controllerActionDescriptor = action.ActionDescriptor as ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            foreach (var arg in controllerActionDescriptor.MethodInfo.GetParameters())
                            {
                                if (arg.CustomAttributes.Any(i => i.AttributeType == typeof(FromFormAttribute)))
                                {
                                    type = arg.ParameterType;
                                    break;
                                }
                            }
                        }

                        
                        if (type != null && validSchemaManager.IsValid(type))
                        {
                            description.RequestSchema = schemaBuilder.GetSchema(type, true);
                            description.RequestSchema.SetDataIsForm(true);
                        }
                    }
                }
            }

            if (queryModelType != null)
            {
                description.QuerySchema = schemaBuilder.GetSchema(queryModelType, true);
            }

            return description;
        }
    }
}
