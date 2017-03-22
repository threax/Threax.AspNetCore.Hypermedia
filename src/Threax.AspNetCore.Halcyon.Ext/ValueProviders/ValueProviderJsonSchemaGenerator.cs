using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class ValueProviderJsonSchemaGenerator : JsonSchemaGenerator
    {
        JsonSchemaGeneratorSettings settings;
        IValueProviderResolver valueProviders;

        public ValueProviderJsonSchemaGenerator(JsonSchemaGeneratorSettings settings, IValueProviderResolver valueProviders)
            : base(settings)
        {
            this.settings = settings;
            this.valueProviders = valueProviders;
        }

        protected override async Task GenerateObjectAsync<TSchemaType>(Type type, TSchemaType schema, NJsonSchema.JsonSchemaResolver schemaResolver)
        {
            await base.GenerateObjectAsync<TSchemaType>(type, schema, schemaResolver);

            foreach (var prop in type.GetTypeInfo().GetProperties())
            {
                var valueProviderAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(ValueProviderAttribute)) as ValueProviderAttribute;
                if (valueProviderAttr != null)
                {
                    var propName = JsonReflectionUtilities.GetPropertyName(prop, Settings.DefaultPropertyNameHandling);
                    var schemaProp = schema.Properties[propName];
                    IValueProvider valueProvider;
                    if (valueProviders.TryGetValueProvider(valueProviderAttr.ProviderType, out valueProvider))
                    {
                        valueProvider.AddExtensions(schemaProp, new ValueProviderArgs(valueProviderAttr, this));
                    }
                    else
                    {
                        throw new ValueProviderException($"Cannot find value provider {valueProviderAttr.ProviderType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                    }
                }
            }
        }
    }
}