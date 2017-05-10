using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.Generation;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// Other state for the value provider.
    /// </summary>
    public class ValueProviderArgs
    {
        public ValueProviderArgs(ValueProviderAttribute attr, JsonSchemaGenerator generator)
        {
            this.ValueProviderAttr = attr;
            this.SchemaGenerator = generator;
        }

        /// <summary>
        /// The attribute on the object that triggered this provider.
        /// </summary>
        public ValueProviderAttribute ValueProviderAttr { get; private set; }

        /// <summary>
        /// The schema generator that the schema was serialized with.
        /// </summary>
        public JsonSchemaGenerator SchemaGenerator { get; private set; }
    }

    public interface IValueProvider
    {
        /// <summary>
        /// Add the extensions for the provided values.
        /// </summary>
        /// <param name="schemaProp">The schema property.</param>
        /// <param name="args">The args.</param>
        /// <returns>Void task.</returns>
        Task AddExtensions(JsonProperty schemaProp, ValueProviderArgs args);
    }
}
