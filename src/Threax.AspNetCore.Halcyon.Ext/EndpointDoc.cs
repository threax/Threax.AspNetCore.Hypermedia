using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// Documentation for a hal endpoint.
    /// </summary>
    [HalModel]
    public class EndpointDoc
    {
        /// <summary>
        /// The json schema for an object that is the body to the request.
        /// </summary>
        [JsonConverter(typeof(SchemaJsonConverter))]
        public JsonSchema4 RequestSchema { get; set; }

        /// <summary>
        /// This will be true if the request schema came from the cache.
        /// </summary>
        public bool RequestSchemaFromCache { get; set; }

        public void SetRequestSchema(EndpointDocCacheResult cacheResult)
        {
            this.RequestSchema = cacheResult.Schema;
            this.RequestSchemaFromCache = cacheResult.FromCache;
        }

        /// <summary>
        /// The json schema for an object that is the body of the response.
        /// </summary>
        [JsonConverter(typeof(SchemaJsonConverter))]
        public JsonSchema4 ResponseSchema { get; set; }

        /// <summary>
        /// This will be true if the response schema came from the cache.
        /// </summary>
        public bool ResponseSchemaFromCache { get; set; }

        public void SetResponseSchema(EndpointDocCacheResult cacheResult)
        {
            this.ResponseSchema = cacheResult.Schema;
            this.ResponseSchemaFromCache = cacheResult.FromCache;
        }

        /// <summary>
        /// True if this object is not empty documentation.
        /// </summary>
        [JsonIgnore]
        public bool HasDocumentation
        {
            get
            {
                return RequestSchema != null || ResponseSchema != null;
            }
        }

        /// <summary>
        /// This will be true if this doc can be cached. This means its docs came from the cache.
        /// </summary>
        [JsonIgnore]
        public bool Cacheable { get; set; }
    }
}
