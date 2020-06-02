using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaBuilder
    {
        Task<JsonSchema4> GetSchema(Type type);

        String GetPropertyName(MemberInfo memberInfo);
    }
}
