using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaBuilder
    {
        JsonSchema4 GetSchema(Type type);
    }
}
