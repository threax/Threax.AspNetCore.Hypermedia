using NJsonSchema;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocJsonSchemaCustomizer
    {
        Task ProcessAsync<TSchemaType>(EndpointDocJsonSchemaCustomizerContext<TSchemaType> context)
            where TSchemaType : JsonSchema4, new();
    }

    public class EndpointDocJsonSchemaCustomizerContext<TSchemaType>
        where TSchemaType : JsonSchema4, new()
    {
        public EndpointDocJsonSchemaGenerator Generator { get; set; }

        public Type Type { get; set; }

        public TSchemaType Schema { get; set; }

        public JsonSchemaResolver SchemaResolver { get; set; }
    }

    /// <summary>
    /// This attribute allows you to further customize a json schema after it has been created. This will be
    /// called after the schema is created for the main type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointDocJsonSchemaCustomizerAttribute : Attribute
    {
        private Type customizerType;
        private IEndpointDocJsonSchemaCustomizer customizer;

        public EndpointDocJsonSchemaCustomizerAttribute(Type customizerType)
        {
            this.customizerType = customizerType;
        }

        public Task ProcessAsync<TSchemaType>(EndpointDocJsonSchemaCustomizerContext<TSchemaType> context)
            where TSchemaType : JsonSchema4, new()
        {
            EnsureCustomizer();
            return customizer.ProcessAsync(context);
        }

        private void EnsureCustomizer()
        {
            if (customizer == null)
            {
                customizer = System.Activator.CreateInstance(customizerType) as IEndpointDocJsonSchemaCustomizer;
                if (customizer == null)
                {
                    throw new InvalidOperationException($"Type {customizerType.FullName} does not implement {nameof(IEndpointDocJsonSchemaCustomizer)}.");
                }
            }
        }
    }
}
