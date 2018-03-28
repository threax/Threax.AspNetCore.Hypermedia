using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public class PagedCollectionViewWithQuery<TItem, TQuery> : PagedCollectionView<TItem>, ICustomJsonSerializer
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

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            var defaultContract = serializer.ContractResolver as DefaultContractResolver;
            var naming = defaultContract?.NamingStrategy;
            if(naming == null) //If we can't get this for some reason, use this class's camel naming.
            {
                naming = camelNaming;
            }
            writer.WriteStartObject();
            foreach (var prop in typeof(TQuery).GetTypeInfo().GetProperties().Where(i => i.Name != nameof(IPagedCollectionQuery.Offset) && i.Name != nameof(IPagedCollectionQuery.Limit)))
            {
                writer.WritePropertyName(naming.GetPropertyName(prop.Name, false));
                serializer.Serialize(writer, prop.GetValue(this.query));
            }
            writer.WritePropertyName(naming.GetPropertyName(nameof(Items), false));
            serializer.Serialize(writer, Items);
            writer.WriteEndObject();
        }

        /// <summary>
        /// This function can be overwritten to add any additional custom query strings needed
        /// to the query url. The base class version just returns url, so there is no need to 
        /// call it from your subclass.
        /// </summary>
        /// <param name="rel">The input rel.</param>
        /// <param name="queryString">The query builder.</param>
        /// <returns>The customized query string.</returns>
        protected override void AddCustomQuery(String rel, QueryStringBuilder queryString)
        {
            //Get all properties except offset and limit
            foreach(var prop in typeof(TQuery).GetTypeInfo().GetProperties().Where(i => i.Name != nameof(IPagedCollectionQuery.Offset) && i.Name != nameof(IPagedCollectionQuery.Limit)))
            {
                var value = prop.GetValue(this.query);
                if (value != null)
                {
                    queryString.AppendItem(camelNaming.GetPropertyName(prop.Name, false), value.ToString());
                }
            }
        }
    }
}
