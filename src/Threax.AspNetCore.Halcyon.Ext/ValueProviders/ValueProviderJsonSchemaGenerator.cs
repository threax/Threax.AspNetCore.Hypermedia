using Newtonsoft.Json.Serialization;
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

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class ValueProviderJsonSchemaGenerator : JsonSchemaGenerator
    {
        JsonSchemaGeneratorSettings settings;
        IValueProviderResolver valueProviders;
        ISchemaCustomizerResolver schemaCustomizers;
        IAutoTitleGenerator titleGenerator;

        public ValueProviderJsonSchemaGenerator(JsonSchemaGeneratorSettings settings, IValueProviderResolver valueProviders, ISchemaCustomizerResolver schemaCustomizers, IAutoTitleGenerator titleGenerator)
            : base(settings)
        {
            this.settings = settings;
            this.valueProviders = valueProviders;
            this.schemaCustomizers = schemaCustomizers;
            this.titleGenerator = titleGenerator;
        }

        protected override async Task GenerateObjectAsync<TSchemaType>(Type type, JsonContract contract, TSchemaType schema, JsonSchemaResolver schemaResolver)
        {
            await base.GenerateObjectAsync<TSchemaType>(type, contract, schema, schemaResolver);

            foreach (var prop in type.GetTypeInfo().GetProperties())
            {
                var propName = JsonReflectionUtilities.GetPropertyName(prop, Settings.DefaultPropertyNameHandling);
                var propType = prop.PropertyType;
                var propTypeInfo = propType.GetTypeInfo();
                var isEnum = propTypeInfo.IsEnum;
                var isNullable = false;
                NJsonSchema.JsonProperty schemaProp;
                if (schema.Properties.TryGetValue(propName, out schemaProp))
                {
                    //Check to see if the value can be null, value types are considered null if they are nullables, 
                    //reference types are considered nullable if they are not marked with a Required attribute.
                    if (propTypeInfo.IsGenericType && propTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>)) //See if the type is a Nullable<T>, this will handle value types
                    {
                        //If this is nullable get the generic arg and use that as the prop type
                        propType = propTypeInfo.GetGenericArguments()[0];
                        propTypeInfo = propType.GetTypeInfo();
                        isEnum = propTypeInfo.IsEnum;
                        isNullable = true;
                    }
                    else
                    {
                        //Check for the Required attribute, if it is not there consider the property to be nullable
                        var requiredAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(RequiredAttribute)) as RequiredAttribute;
                        if (requiredAttr == null)
                        {
                            isNullable = true;
                        }
                    }

                    var valueProviderAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(ValueProviderAttribute)) as ValueProviderAttribute;
                    if (valueProviderAttr != null) //If the user gives a value provider, use it
                    {
                        IValueProvider valueProvider;
                        if (valueProviders.TryGetValueProvider(valueProviderAttr.ProviderType, out valueProvider))
                        {
                            await valueProvider.AddExtensions(schemaProp, new ValueProviderArgs(valueProviderAttr, this, isNullable, prop));
                        }
                        else
                        {
                            throw new ValueProviderException($"Cannot find value provider {valueProviderAttr.ProviderType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                        }
                    }
                    else if (isEnum) //If there is no value provider and the value is an enum, use the enum values automaticaly
                    {
                        //For some reason enums do not get the custom attributes, so do it here
                        foreach (var attr in prop.GetCustomAttributes().Select(i => i as JsonSchemaExtensionDataAttribute).Where(i => i != null))
                        {
                            if (schemaProp.ExtensionData == null)
                            {
                                schemaProp.ExtensionData = new Dictionary<String, Object>();
                            }
                            if (!schemaProp.ExtensionData.ContainsKey(attr.Property))
                            {
                                schemaProp.ExtensionData.Add(attr.Property, attr.Value);
                            }
                        }

                        //Cleanup stuff we are not supporting right now (oneOf, anyOf, not etc).
                        schemaProp.AllOf.Clear();
                        schemaProp.AnyOf.Clear();
                        schemaProp.Not = null;
                        schemaProp.OneOf.Clear();

                        var labelProvider = new EnumLabelValuePairProvider(propType);
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
                        await labelProvider.AddExtensions(schemaProp, new ValueProviderArgs(new ValueProviderAttribute(typeof(Object)), this, isNullable, prop));
                    }

                    //Handle any schema customizations
                    var schemaCustomizerAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(CustomizeSchemaAttribute)) as CustomizeSchemaAttribute;
                    if (schemaCustomizerAttr != null)
                    {
                        ISchemaCustomizer customizer;
                        if (schemaCustomizers.TryGetValueProvider(schemaCustomizerAttr.CustomizerType, out customizer))
                        {
                            await customizer.Customize(new SchemaCustomizerArgs(propName, prop, schemaProp, schema));
                        }
                        else
                        {
                            throw new ValueProviderException($"Cannot find schema customizer {schemaCustomizerAttr.CustomizerType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                        }
                    }

                    //Attempt to generate a nice title for the property if no title was set automatically
                    if (schemaProp.Title == null)
                    {
                        schemaProp.Title = titleGenerator.CreateTitle(schemaProp.Name);
                    }
                }
            }
        }
    }
}