using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Halcyon.HAL.Attributes;
using NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocBuilder
    {
        EndpointDoc GetDoc(String groupName, String method, String relativePath);
    }

    public class EndpointDocBuilder : IEndpointDocBuilder
    {
        IApiDescriptionGroupCollectionProvider descriptionProvider;

        public EndpointDocBuilder(IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            this.descriptionProvider = descriptionProvider;
        }

        public virtual EndpointDoc GetDoc(String groupName, String method, String relativePath)
        {
            if (relativePath != null && (relativePath.EndsWith("/") || relativePath.EndsWith("\\")))
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            var group = descriptionProvider.ApiDescriptionGroups.Items.First(i => i.GroupName == groupName);
            var action = group.Items.First(i => i.HttpMethod == method && i.RelativePath == relativePath);

            var description = new EndpointDoc();
            foreach (var param in action.ParameterDescriptions)
            {
                if (param.Source.IsFromRequest && param.Source.Id == "Body")
                {
                    description.RequestSchema = GetSchema(param.Type);
                }
            }

            var controllerActionDesc = action.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDesc != null)
            {
                var methodInfo = controllerActionDesc.MethodInfo;
                if (methodInfo.ReturnType != typeof(void))
                {
                    description.ResponseSchema = GetSchema(methodInfo.ReturnType);
                }
            }

            return description;
        }

        protected virtual JsonSchema4 GetSchema(Type type)
        {
            //See if we are returning a task, and get its type
            if (typeof(IAsyncResult).IsAssignableFrom(type))
            {
                if (type.GenericTypeArguments.Length == 0)
                {
                    //If we are a task with no generic args, this is the equivalent of void, return null
                    return null;
                }
                type = type.GenericTypeArguments.First();
            }

            //Also make sure we have a HalModelAttribute on the class. 
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.GetCustomAttribute<HalModelAttribute>() == null)
            {
                throw new InvalidOperationException($"{type.Name} is not a valid schema object. Declare a HalModel attribute on it to mark it valid.");
            }

            //Finally return the schema
            var t = JsonSchema4.FromTypeAsync(type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings()
            {
                DefaultEnumHandling = EnumHandling.String,
                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase,
                FlattenInheritanceHierarchy = true
            });
            t.Wait();
            return t.Result;
        }
    }
}
