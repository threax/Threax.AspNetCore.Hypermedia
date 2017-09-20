using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.Generation;
using System.Reflection;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// Other state for the value provider.
    /// </summary>
    public class ValueProviderArgs
    {
        public ValueProviderArgs(ValueProviderAttribute attr, JsonSchemaGenerator generator, bool isNullable, PropertyInfo propertyInfo)
        {
            this.ValueProviderAttr = attr;
            this.SchemaGenerator = generator;
            this.IsNullable = isNullable;
            this.PropertyInfo = propertyInfo;
        }

        /// <summary>
        /// The attribute on the object that triggered this provider.
        /// </summary>
        public ValueProviderAttribute ValueProviderAttr { get; private set; }

        /// <summary>
        /// The schema generator that the schema was serialized with.
        /// </summary>
        public JsonSchemaGenerator SchemaGenerator { get; private set; }

        /// <summary>
        /// If this is true the value can be null.
        /// </summary>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// The property info for the property having values provided.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
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
