using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using NJsonSchema;
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
        /// The json schema for an object that represents the query to the request.
        /// </summary>
        [JsonConverter(typeof(SchemaJsonConverter))]
        public JsonSchema4 QuerySchema { get; set; }

        /// <summary>
        /// The json schema for an object that is the body to the request.
        /// </summary>
        [JsonConverter(typeof(SchemaJsonConverter))]
        public JsonSchema4 RequestSchema { get; set; }

        /// <summary>
        /// The json schema for an object that is the body of the response.
        /// </summary>
        [JsonConverter(typeof(SchemaJsonConverter))]
        public JsonSchema4 ResponseSchema { get; set; }

        /// <summary>
        /// True if this object is not empty documentation.
        /// </summary>
        [JsonIgnore]
        public bool HasDocumentation
        {
            get
            {
                return QuerySchema != null || RequestSchema != null || ResponseSchema != null;
            }
        }
    }
}
