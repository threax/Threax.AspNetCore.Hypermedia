using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaFinder
    {
        String Find(Type schema);

        String Find(string schema);
    }
}