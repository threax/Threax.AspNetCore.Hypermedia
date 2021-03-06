﻿using Halcyon;
using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This class will use reflection to include the properties of the query object on the collection without
    /// needing to define this manually.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    /// <typeparam name="TQuery">The type of the query for the collection.</typeparam>
    [JsonConverter(typeof(CustomJsonSerializerConverter))]
    [EndpointDocJsonSchemaCustomizer(typeof(PagedCollectionViewWithQueryEndpointDocJsonSchemaCustomizer))]
    public class PagedCollectionViewWithQuery<TItem, TQuery> : PagedCollectionView<TItem>, ICustomJsonSerializer, ICustomVardicPropertyProvider
        where TQuery : IPagedCollectionQuery
    {
        //Use the newtonsoft camel case conversion, this way we are consistent when generating camel case names
        private static CamelCaseNamingStrategy camelNaming = new CamelCaseNamingStrategy();

        private TQuery query;

        public PagedCollectionViewWithQuery(TQuery query, int total, IEnumerable<TItem> items)
            :base(query, total, items)
        {
            this.query = query;
        }

        public void AddCustomVardicProperties(IDictionary<string, object> vardic)
        {
            foreach (var prop in GetQueryProperties())
            {
                var objValue = prop.GetValue(query, null);

                vardic[prop.Name] = objValue;
            }
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            var defaultContract = serializer.ContractResolver as DefaultContractResolver;
            var naming = defaultContract?.NamingStrategy;
            if(naming == null) //If we can't get this for some reason, use this class's camel naming.
            {
                naming = camelNaming;
            }
            writer.WriteStartObject();
            foreach (var prop in GetQueryProperties())
            {
                writer.WritePropertyName(naming.GetPropertyName(prop.Name, false));
                serializer.Serialize(writer, prop.GetValue(this.query));
            }
            foreach (var prop in GetCollectionProperties())
            {
                writer.WritePropertyName(naming.GetPropertyName(prop.Name, false));
                serializer.Serialize(writer, prop.GetValue(this));
            }
            writer.WriteEndObject();
        }

        protected IEnumerable<PropertyInfo> GetQueryProperties()
        {
            return typeof(TQuery).GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        protected IEnumerable<PropertyInfo> GetCollectionProperties()
        {
            //All properties on this class except offset and limit, which are already in the query
            return this.GetType().GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(i => i.Name != nameof(PagedCollectionView<Object>.Offset) && 
                       i.Name != nameof(PagedCollectionView<Object>.Limit) && 
                       i.Name != nameof(PagedCollectionView<Object>.Items) &&
                       i.Name != nameof(PagedCollectionView<Object>.AsObjects) &&
                       i.Name != nameof(PagedCollectionView<Object>.CollectionType));
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. The base class version just returns url, so there is no need to 
        /// call it from your subclass.
        /// </summary>
        /// <param name="rel">The input rel.</param>
        /// <param name="queryString">The query builder.</param>
        /// <returns>The customized query string.</returns>
        protected override void AddCustomQuery(String rel, RequestDataBuilder queryString)
        {
            //Get all properties except offset and limit
            foreach(var prop in typeof(TQuery).GetTypeInfo().GetProperties().Where(i => i.Name != nameof(IPagedCollectionQuery.Offset) && i.Name != nameof(IPagedCollectionQuery.Limit)))
            {
                var value = prop.GetValue(this.query);
                if (value != null)
                {
                    var name = camelNaming.GetPropertyName(prop.Name, false);
                    var collection = value as System.Collections.ICollection;
                    if (collection != null)
                    {
                        foreach(var item in collection)
                        {
                            queryString.AppendItem(name, item?.ToString());
                        }
                    }
                    else
                    {
                        queryString.AppendItem(name, value.ToString());
                    }
                }
            }
        }
    }

    class PagedCollectionViewWithQueryEndpointDocJsonSchemaCustomizer : IEndpointDocJsonSchemaCustomizer
    {
        private static Type pagedCollectionWithViewGenericType = typeof(PagedCollectionViewWithQuery<,>).GetGenericTypeDefinition();

        public IEnumerable<PropertyInfo> GetAdditionalPropertyInfo()
        {
            yield break;
        }

        public async Task ProcessAsync<TSchemaType>(EndpointDocJsonSchemaCustomizerContext<TSchemaType> context)
            where TSchemaType : JsonSchema4, new()
        {
            //Walk the type to find the PagedCollectionViewWithQuery
            var pagedCollectionType = context.Type;
            while(pagedCollectionType != null)
            {
                if (pagedCollectionType.IsGenericType)
                {
                    if (pagedCollectionType.GetGenericTypeDefinition() == pagedCollectionWithViewGenericType)
                    {
                        break;
                    }
                }
                pagedCollectionType = pagedCollectionType.BaseType;
            }
            if(pagedCollectionType == null)
            {
                throw new InvalidOperationException($"Could not find {nameof(PagedCollectionViewWithQuery<Object, PagedCollectionQuery>)} superclass on {context.Type.Name}. That superclass must be implemented to use the {nameof(PagedCollectionViewWithQueryEndpointDocJsonSchemaCustomizer)}.");
            }

            var queryType = pagedCollectionType.GenericTypeArguments[1];

            //Remove offset and limit from schema, the query will add them back
            context.Schema.Properties.Remove("offset");
            context.Schema.Properties.Remove("limit");

            var originalTitle = context.Schema.Title;

            await context.Generator.GenerateAsync<TSchemaType>(queryType, Enumerable.Empty<Attribute>(), context.Schema, context.SchemaResolver);

            context.Schema.Title = originalTitle;
        }
    }
}
