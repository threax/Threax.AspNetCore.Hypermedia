using Halcyon.HAL.Attributes;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [HalModel]
    public class EndpointDoc
    {
        public JsonSchema4 QuerySchema { get; set; }

        public JsonSchema4 RequestSchema { get; set; }

        public JsonSchema4 ResponseSchema { get; set; }
    }
}
