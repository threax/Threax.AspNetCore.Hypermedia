using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalEndpointDoc
    {
        /// <summary>
        /// The json schema for an object that represents the query to the request.
        /// </summary>
        public JsonSchema4 QuerySchema { get; set; }

        /// <summary>
        /// The json schema for an object that is the body to the request.
        /// </summary>
        public JsonSchema4 RequestSchema { get; set; }

        /// <summary>
        /// The json schema for an object that is the body of the response.
        /// </summary>
        public JsonSchema4 ResponseSchema { get; set; }
    }
}
