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
                        await valueProvider.AddExtensions(schemaProp, new ValueProviderArgs(valueProviderAttr, this));
                    }
                    else
                    {
                        throw new ValueProviderException($"Cannot find value provider {valueProviderAttr.ProviderType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                    }
                }
                else //Check for and handle enums
                {
                    var propType = prop.PropertyType;
                    var propTypeInfo = propType.GetTypeInfo();
                    var isEnum = propTypeInfo.IsEnum;
                    var isNullableEnum = false;
                    if (!isEnum)
                    {
                        //If already found to be an enum, will not be a nullable enum, so skip that check
                        if(propTypeInfo.IsGenericType &&
                            propTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            //If this is nullable check the generic arg
                            var nullableArgType = propTypeInfo.GetGenericArguments()[0];
                            if (nullableArgType.GetTypeInfo().IsEnum)
                            {
                                //If the first arg is an enum change our type to be that
                                isNullableEnum = true;
                                propType = nullableArgType;
                                propTypeInfo = nullableArgType.GetTypeInfo();
                            }
                        }
                    }
                    if (isEnum || isNullableEnum)
                    {
                        var propName = JsonReflectionUtilities.GetPropertyName(prop, Settings.DefaultPropertyNameHandling);
                        var schemaProp = schema.Properties[propName];

                        //Cleanup stuff we are not supporting right now (oneOf, anyOf, not etc).
                        schemaProp.AllOf.Clear();
                        schemaProp.AnyOf.Clear();
                        schemaProp.Not = null;
                        schemaProp.OneOf.Clear();

                        var labelProvider = new EnumLabelValuePairProvider(propType, prop, isNullableEnum);
                        switch (settings.DefaultEnumHandling)
                        {
                            case EnumHandling.Integer:
                                schemaProp.Type = JsonObjectType.Integer;
                                break;
                            case EnumHandling.String:
                            default:
                                schemaProp.Type = JsonObjectType.String;
                                break;
                        }
                        await labelProvider.AddExtensions(schemaProp, new ValueProviderArgs(new ValueProviderAttribute(typeof(Object)), this));
                    }
                }
            }
        }
    }
}