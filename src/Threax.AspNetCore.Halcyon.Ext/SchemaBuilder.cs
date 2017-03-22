using Halcyon.HAL.Attributes;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaBuilder : ISchemaBuilder
    {
        JsonSchemaGeneratorSettings settings;

        public SchemaBuilder(JsonSchemaGeneratorSettings settings)
        {
            this.settings = settings;
        }

        public JsonSchema4 GetSchema(Type type)
        {
            //See if we are returning a task, and get its type
            if (typeof(IAsyncResult).IsAssignableFrom(type))
            {
                if (type.GenericTypeArguments.Length == 0)
                {
                    //If we are a task with no generic args, this is the equivalent of void, return null
                    return null;
                }
                type = type.GenericTypeArguments.First();
            }

            //Also make sure we have a HalModelAttribute on the class. 
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.GetCustomAttribute<HalModelAttribute>() == null)
            {
                throw new InvalidOperationException($"{type.Name} is not a valid schema object. Declare a HalModel attribute on it to mark it valid.");
            }

            //Finally return the schema
            var t = JsonSchema4.FromTypeAsync(type, settings);
            t.Wait();
            return t.Result;
        }
    }
}
