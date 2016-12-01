using Halcyon.HAL.Attributes;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaFinder : ISchemaFinder
    {
        public JsonSchema4 Find(String schema)
        {
            //Restrict to only the View Model namespace.
            var type = Type.GetType(schema);
            if (type == null)
            {
                throw new InvalidOperationException($"Cannot find type {schema}.");
            }

            //Also make sure we have a HalModelAttribute on the class. 
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.GetCustomAttribute<HalModelAttribute>() == null)
            {
                throw new InvalidOperationException($"Cannot find type {schema}.");
            }

            //Finally return the schema
            return JsonSchema4.FromType(type);
        }
    }
}
