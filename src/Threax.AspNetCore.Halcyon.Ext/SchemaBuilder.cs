using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Generation.TypeMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class SchemaBuilder : ISchemaBuilder
    {
        JsonSchemaGenerator generator;
        IValidSchemaTypeManager validSchemaManager;

        public SchemaBuilder(JsonSchemaGenerator generator, IValidSchemaTypeManager validSchemaManager)
        {
            this.generator = generator;
            this.validSchemaManager = validSchemaManager;
        }

        public JsonSchema4 GetSchema(Type type, bool allowEnumerables = false)
        {
            bool isEnumerable = false;

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

            //If we allow collections check the collection type
            if (typeof(IEnumerable).IsAssignableFrom(type) && type.GenericTypeArguments.Length > 0)
            {
                type = type.GenericTypeArguments.First();
                isEnumerable = true;
            }

            //Handle action results special, mark an extension in the schema 
            if (typeof(IActionResult).IsAssignableFrom(type))
            {
                var schema = new JsonSchema4()
                {
                    Title = "Response",
                };
                schema.SetRawResponse(true);
                return schema;
            }

            //Also make sure we have a HalModelAttribute on the class. 
            if (!validSchemaManager.IsValid(type))
            {
                throw new InvalidOperationException($"{type.Name} is not a valid schema object. {validSchemaManager.ErrorMessage}");
            }

            //Create schema from type
            var t = generator.GenerateAsync(type);
            t.Wait();

            //Set the array extension to true if this was enumerable
            t.Result.SetIsArray(isEnumerable);

            return t.Result;
        }
    }
}
