using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    /// <summary>
    /// This class provides a way to gather up interfaces to generate easily.
    /// </summary>
    class InterfaceManager
    {
        private Dictionary<String, JsonSchema4> interfacesToWrite = new Dictionary<string, JsonSchema4>();

        public InterfaceManager()
        {

        }

        public void Add(JsonSchema4 schema)
        {
            if (!interfacesToWrite.ContainsKey(schema.Title))
            {
                interfacesToWrite.Add(schema.Title, schema);
            }
        }

        public IDictionary<String, JsonSchema4> Interfaces
        {
            get
            {
                return interfacesToWrite;
            }
        }

        /// <summary>
        /// Get the first schema, need this to kick off the generator. Can be null, which means there are no schemas
        /// </summary>
        public JsonSchema4 FirstSchema
        {
            get
            {
                return interfacesToWrite.Values.FirstOrDefault();
            }
        }
    }
}
