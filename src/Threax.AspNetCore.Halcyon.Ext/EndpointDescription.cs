using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Halcyon.HAL;
using Newtonsoft.Json;
using NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [HalModel]
    public class EndpointDescription
    {
        public JsonSchema4 RequestSchema { get; set; }

        public JsonSchema4 ResponseSchema { get; set; }
    }
}
