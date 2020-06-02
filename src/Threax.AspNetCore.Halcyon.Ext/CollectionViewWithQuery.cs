using Halcyon;
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
    [EndpointDocJsonSchemaCustomizer(typeof(CollectionViewWithQueryEndpointDocJsonSchemaCustomizer))]
    public class CollectionViewWithQuery<TItem, TQuery> : CollectionView<TItem>, ICustomJsonSerializer, ICustomVardicPropertyProvider, IQueryStringProvider
    {
        //Use the newtonsoft camel case conversion, this way we are consistent when generating camel case names
        private static CamelCaseNamingStrategy camelNaming = new CamelCaseNamingStrategy();

        private TQuery query;

        public CollectionViewWithQuery(TQuery query, int total, IEnumerable<TItem> items)
            :base(items)
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
                .Where(i => 
                       i.Name != nameof(CollectionView<Object>.Items) &&
                       i.Name != nameof(CollectionView<Object>.AsObjects) &&
                       i.Name != nameof(CollectionView<Object>.CollectionType));
        }

        /// <summary>
        /// This will add all the properties from the query class to the query for this collection.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="queryString"></param>
        public virtual void AddQuery(string rel, RequestDataBuilder queryString)
        {
            //Get all properties except offset and limit
            foreach (var prop in typeof(TQuery).GetTypeInfo().GetProperties())
            {
                var value = prop.GetValue(this.query);
                if (value != null)
                {
                    var name = camelNaming.GetPropertyName(prop.Name, false);
                    var collection = value as System.Collections.ICollection;
                    if (collection != null)
                    {
                        foreach (var item in collection)
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

    class CollectionViewWithQueryEndpointDocJsonSchemaCustomizer : IEndpointDocJsonSchemaCustomizer
    {
        private static Type CollectionWithViewGenericType = typeof(CollectionViewWithQuery<,>).GetGenericTypeDefinition();

        public IEnumerable<PropertyInfo> GetAdditionalPropertyInfo()
        {
            yield break;
        }

        public async Task ProcessAsync<TSchemaType>(EndpointDocJsonSchemaCustomizerContext<TSchemaType> context)
            where TSchemaType : JsonSchema4, new()
        {
            //Walk the type to find the CollectionViewWithQuery
            var CollectionType = context.Type;
            while(CollectionType != null)
            {
                if (CollectionType.IsGenericType)
                {
                    if (CollectionType.GetGenericTypeDefinition() == CollectionWithViewGenericType)
                    {
                        break;
                    }
                }
                CollectionType = CollectionType.BaseType;
            }
            if(CollectionType == null)
            {
                throw new InvalidOperationException($"Could not find {nameof(CollectionViewWithQuery<Object, Object>)} superclass on {context.Type.Name}. That superclass must be implemented to use the {nameof(CollectionViewWithQueryEndpointDocJsonSchemaCustomizer)}.");
            }

            var queryType = CollectionType.GenericTypeArguments[1];

            //Remove offset and limit from schema, the query will add them back
            context.Schema.Properties.Remove("offset");
            context.Schema.Properties.Remove("limit");

            var originalTitle = context.Schema.Title;

            await context.Generator.GenerateAsync<TSchemaType>(queryType, Enumerable.Empty<Attribute>(), context.Schema, context.SchemaResolver);

            context.Schema.Title = originalTitle;
        }
    }
}
