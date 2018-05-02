using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaBuilder
    {
        Task<JsonSchema4> GetSchema(Type type, bool allowCollections = false);

        String GetPropertyName(MemberInfo memberInfo);
    }
}
