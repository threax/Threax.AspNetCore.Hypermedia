using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class EndpointDocJsonSchemaGenerator : JsonSchemaGenerator
    {
        private static readonly Newtonsoft.Json.Serialization.JsonProperty DummyProperty = new Newtonsoft.Json.Serialization.JsonProperty();

        JsonSchemaGeneratorSettings settings;
        IValueProviderResolver valueProviders;
        ISchemaCustomizerResolver schemaCustomizers;
        IAutoTitleGenerator titleGenerator;
        private bool useValueProviders = true;

        public EndpointDocJsonSchemaGenerator(JsonSchemaGeneratorSettings settings, IValueProviderResolver valueProviders, ISchemaCustomizerResolver schemaCustomizers, IAutoTitleGenerator titleGenerator)
            : base(settings)
        {
            this.settings = settings;
            this.valueProviders = valueProviders;
            this.schemaCustomizers = schemaCustomizers;
            this.titleGenerator = titleGenerator;
        }

        public bool UseValueProviders { get => useValueProviders; set => useValueProviders = value; }

        protected override async Task GenerateObjectAsync<TSchemaType>(Type type, TSchemaType schema, JsonSchemaResolver schemaResolver)
        {
            await base.GenerateObjectAsync<TSchemaType>(type, schema, schemaResolver);
            var processedReferences = new List<JsonSchema4>();

            foreach (var prop in type.GetTypeInfo().GetProperties())
            {
                var propName = GetPropertyName(DummyProperty, prop); //Using dummy property here, the call in the superclass will look at the member info first (v9.9.10)
                NJsonSchema.JsonProperty schemaProp;
                if (schema.Properties.TryGetValue(propName, out schemaProp))
                {
                    var propType = prop.PropertyType;
                    var propTypeInfo = propType.GetTypeInfo();
                    var isArray = (schemaProp.Type & JsonObjectType.Array) != 0;
                    var isNullable = false;
                    Type enumType = null;
                    if (propTypeInfo.IsEnum)
                    {
                        enumType = propType;
                    }

                    //Always make sure we have extension data declared
                    if (schemaProp.ExtensionData == null)
                    {
                        schemaProp.ExtensionData = new Dictionary<String, Object>();
                    }

                    //Check to see if the value can be null, value types are considered null if they are nullables, 
                    //reference types are considered nullable if they are not marked with a Required attribute.
                    if (propTypeInfo.IsGenericType && propTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>)) //See if the type is a Nullable<T>, this will handle value types
                    {
                        //If this is nullable get the generic arg and use that as the prop type
                        propType = propTypeInfo.GetGenericArguments()[0];
                        propTypeInfo = propType.GetTypeInfo();
                        if (propTypeInfo.IsEnum)
                        {
                            enumType = propType;
                        }
                        isNullable = true;
                    }
                    else if (enumType == null) //Skip enum types, those should be nullable, otherwise they are required.
                    {
                        //Check for the Required attribute, if it is not there consider the property to be nullable if it is not a value type
                        var requiredAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(RequiredAttribute)) as RequiredAttribute;
                        if (requiredAttr == null)
                        {
                            isNullable = !propTypeInfo.IsValueType;
                        }

                        //Also, if this item is an array, see if it an array of enums
                        if (isArray && propTypeInfo.IsGenericType && typeof(ICollection).IsAssignableFrom(propTypeInfo.GetGenericTypeDefinition()))
                        {
                            var testEnumType = propTypeInfo.GetGenericArguments()[0];
                            var testEnumTypeInfo = testEnumType.GetTypeInfo();
                            if (testEnumTypeInfo.IsEnum)
                            {
                                enumType = testEnumType;
                            }
                        }
                    }

                    //Check for value providers
                    //These are always used, if the user does this on an enum these values override the ones in the definition.
                    var valueProviderAttr = prop.GetCustomAttributes().FirstOrDefault(i => i.GetType() == typeof(ValueProviderAttribute)) as ValueProviderAttribute;
                    if (valueProviderAttr != null && useValueProviders)
                    {
                        ValueProviders.IValueProvider valueProvider;
                        if (valueProviders.TryGetValueProvider(valueProviderAttr.ProviderType, out valueProvider))
                        {
                            await valueProvider.AddExtensions(schemaProp, new ValueProviderArgs(valueProviderAttr, this, isNullable, prop));
                        }
                        else
                        {
                            throw new ValueProviderException($"Cannot find value provider {valueProviderAttr.ProviderType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                        }
                    }

                    //If the type is an enum, load it correctly
                    if (enumType != null) 
                    {
                        //The enum typeSchema gets these properties, so load them again here onto the actual schema prop
                        foreach (var attr in prop.GetCustomAttributes().Select(i => i as JsonSchemaExtensionDataAttribute).Where(i => i != null))
                        {
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

                        //If the prop is an enum with a reference use our value provider to load the values into the enum definition.
                        JsonSchema4 typeSchema;
                        if (isArray)
                        {
                            typeSchema = schemaProp.Item.ActualTypeSchema;
                        }
                        else
                        {
                            typeSchema = schemaProp.ActualTypeSchema;
                        }

                        if (typeSchema != null && !processedReferences.Contains(typeSchema))
                        {
                            processedReferences.Add(typeSchema);
                            typeSchema.Enumeration?.Clear();
                            typeSchema.EnumerationNames?.Clear();
                            var labelProvider = new EnumLabelValuePairProvider(enumType);
                            await labelProvider.AddExtensions(typeSchema, new ValueProviderArgs(new ValueProviderAttribute(typeof(Object)), this, isNullable, prop));
                        }
                    }

                    //Handle any schema customizations
                    var schemaCusomizerArgs = new SchemaCustomizerArgs(propName, prop, schemaProp, schema, this, type);
                    foreach (var schemaCustomizerAttr in prop.GetCustomAttributes().Where(i => typeof(CustomizeSchemaAttribute).IsAssignableFrom(i.GetType())).Select(i => i as CustomizeSchemaAttribute))
                    {
                        ISchemaCustomizer customizer = schemaCustomizerAttr as ISchemaCustomizer; //Allow the customizer to also directly implement the ISchemaCustomizer interface, if so use it directly
                        if (customizer == null) //Otherwise look up the customizer, which allows for dependency injection into the customizer
                        {
                            if (!schemaCustomizers.TryGetValueProvider(schemaCustomizerAttr.CustomizerType, out customizer))
                            {
                                throw new ValueProviderException($"Cannot find schema customizer {schemaCustomizerAttr.CustomizerType.Name}. It needs to be registered in the IValueProviderResolver or in services by default.");
                            }
                        }

                        if (customizer != null)
                        {
                            await customizer.Customize(schemaCusomizerArgs);
                        }
                    }

                    foreach (var multiExtension in prop.GetCustomAttributes().Where(i => typeof(JsonSchemaMultiExtensionDataAttribute).IsAssignableFrom(i.GetType())).Cast<JsonSchemaMultiExtensionDataAttribute>())
                    {
                        foreach (var item in multiExtension.ExtensionValues)
                        {
                            schemaProp.ExtensionData[item.Key] = item.Value;
                        }
                    }

                    //Attempt to generate a nice title for the property if no title was set automatically
                    if (schemaProp.Title == null)
                    {
                        schemaProp.Title = titleGenerator.CreateTitle(schemaProp.Name);
                    }
                }
            }

            var typeCustomizer = type.GetCustomAttributes<EndpointDocJsonSchemaCustomizerAttribute>().FirstOrDefault();
            if (typeCustomizer != null)
            {
                await typeCustomizer.ProcessAsync<TSchemaType>(new EndpointDocJsonSchemaCustomizerContext<TSchemaType>()
                {
                    Generator = this,
                    Schema = schema,
                    SchemaResolver = schemaResolver,
                    Type = type
                });
            }
        }
    }
}