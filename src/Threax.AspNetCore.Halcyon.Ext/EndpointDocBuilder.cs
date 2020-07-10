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
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// A common query class that can be used in doc controllers.
    /// </summary>
    public class EndpointDocQuery
    {
        /// <summary>
        /// Set this to true (default) to include the request docs if available.
        /// </summary>
        public bool IncludeRequest { get; set; } = true;

        /// <summary>
        /// Set this to true (default) to include the response docs if available.
        /// </summary>
        public bool IncludeResponse { get; set; } = true;
    }

    public class EndpointDocBuilderOptions
    {
        /// <summary>
        /// Include a user to throw an UnauthorizedAccessException if the user cannot access the endpoint.
        /// </summary>
        public ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Include the request docs in the results. Default: true.
        /// </summary>
        public bool IncludeRequest { get; set; } = true;

        /// <summary>
        /// Include the response docs in the results. Default: true.
        /// </summary>
        public bool IncludeResponse { get; set; } = true;
    }

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
        [Obsolete("This version of GetDoc is obsolete. Please use another overload", false)]
        Task<EndpointDoc> GetDoc(String groupName, String method, String relativePath, ClaimsPrincipal user = null);

        /// <summary>
        /// Get the EndpointDoc object for the specified endpoint
        /// </summary>
        /// <param name="groupName">The group name</param>
        /// <param name="method">The method name</param>
        /// <param name="relativePath">The relative path</param>
        /// <returns>The EndpointDoc object for the endpoint.</returns>
        Task<EndpointDoc> GetDoc(String groupName, String method, String relativePath);

        /// <summary>
        /// Get the EndpointDoc object for the specified endpoint
        /// </summary>
        /// <param name="groupName">The group name</param>
        /// <param name="method">The method name</param>
        /// <param name="relativePath">The relative path</param>
        /// <param name="options">Additional options for generating docs.</param>
        /// <returns>The EndpointDoc object for the endpoint.</returns>
        Task<EndpointDoc> GetDoc(String groupName, String method, String relativePath, EndpointDocBuilderOptions options);
    }

    public class EndpointDocBuilder : IEndpointDocBuilder
    {
        private readonly IApiDescriptionGroupCollectionProvider descriptionProvider;
        private readonly ISchemaBuilder schemaBuilder;
        private readonly IValidSchemaTypeManager validSchemaManager;
        private readonly IEndpointDocCache endpointDocCache;

        public EndpointDocBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider, ISchemaBuilder schemaBuilder, IValidSchemaTypeManager validSchemaManager, IEndpointDocCache endpointDocCache)
        {
            this.descriptionProvider = descriptionProvider;
            this.schemaBuilder = schemaBuilder;
            this.validSchemaManager = validSchemaManager;
            this.endpointDocCache = endpointDocCache;
        }

        public Task<EndpointDoc> GetDoc(String groupName, String method, String relativePath, ClaimsPrincipal user = null)
        {
            return this.GetDoc(groupName, method, relativePath, new EndpointDocBuilderOptions()
            {
                User = user
            });
        }

        public Task<EndpointDoc> GetDoc(string groupName, string method, string relativePath)
        {
            return this.GetDoc(groupName, method, relativePath, new EndpointDocBuilderOptions());
        }

        public async Task<EndpointDoc> GetDoc(string groupName, string method, string relativePath, EndpointDocBuilderOptions options)
        {
            if (relativePath == null)
            {
                relativePath = "";
            }
            else if (relativePath.EndsWith("/") || relativePath.EndsWith("\\"))
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            var group = descriptionProvider.ApiDescriptionGroups.Items.FirstOrDefault(i => i.GroupName == groupName);
            if (group == null)
            {
                throw new InvalidOperationException($"Cannot find an api group for {groupName}. Did you declare a route to that group?");
            }

            var action = group.Items.FirstOrDefault(i => i.HttpMethod == method && i.RelativePath == relativePath);
            if (action == null)
            {
                throw new InvalidOperationException($"Cannot find an api action for {relativePath} and method {method} in api group {groupName}.");
            }

            var description = new EndpointDoc();
            bool handleFormData = true;

            if (options.IncludeResponse)
            {
                var controllerActionDesc = action.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDesc != null)
                {
                    var methodInfo = controllerActionDesc.MethodInfo;

                    //Check to see if the user can actually access the endpoint we requested
                    if (options.User != null && !HalcyonExtUtils.CanUserAccess(options.User, methodInfo, controllerActionDesc.ControllerTypeInfo))
                    {
                        throw new UnauthorizedAccessException("User cannot access requested endpoint");
                    }

                    var returnType = methodInfo.ReturnType;
                    if (returnType != typeof(void))
                    {
                        description.ResponseSchema = await endpointDocCache.GetCachedSchema(returnType, schemaBuilder.GetSchema);
                    }
                }
            }

            if (options.IncludeRequest)
            {
                foreach (var param in action.ParameterDescriptions)
                {
                    if (param.Source.IsFromRequest)
                    {
                        if (param.Source.CanAcceptDataFrom(BindingSource.Body))
                        {
                            description.RequestSchema = await endpointDocCache.GetCachedSchema(param.Type, schemaBuilder.GetSchema);
                        }
                        else if (param.Source.CanAcceptDataFrom(BindingSource.Query))
                        {
                            description.RequestSchema = await endpointDocCache.GetCachedSchema(param.ModelMetadata.ContainerType, schemaBuilder.GetSchema);
                        }
                        else if (handleFormData && param.Source.CanAcceptDataFrom(BindingSource.Form))
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
                                description.RequestSchema = await endpointDocCache.GetCachedSchema(type, schemaBuilder.GetSchema);
                                description.RequestSchema.SetDataIsForm(true);
                            }
                        }
                    }
                }
            }

            return description;
        }
    }
}
