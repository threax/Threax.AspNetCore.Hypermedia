using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Halcyon.HAL;
using Newtonsoft.Json;
using NJsonSchema;
using Newtonsoft.Json.Linq;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [HalModel]
    public class EndpointDescription
    {
        public JObject RequestSchema { get; set; }

        public JObject ResponseSchema { get; set; }
    }
}
