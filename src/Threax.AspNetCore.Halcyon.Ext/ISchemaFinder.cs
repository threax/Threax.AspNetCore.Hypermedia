using NJsonSchema;
using System;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaFinder
    {
        JsonSchema4 Find(Type schema);

        JsonSchema4 Find(string schema);
    }
}