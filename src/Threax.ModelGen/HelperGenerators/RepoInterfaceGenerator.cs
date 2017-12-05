using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class RepoInterfaceGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models, schema.GetKeyType().Name);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String modelIdType) {
            return
$@"using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using {ns}.InputModels;
using {ns}.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace {ns}.Repository
{{
    public partial interface I{Model}Repository
    {{
        Task<{Model}> Add({Model}Input value);
        Task AddRange(IEnumerable<{Model}Input> values);
        Task Delete({modelIdType} id);
        Task<{Model}> Get({modelIdType} {model}Id);
        Task<bool> Has{Models}();
        Task<{Model}Collection> List({Model}Query query);
        Task<{Model}> Update({modelIdType} {model}Id, {Model}Input value);
    }}
}}";
        }
    }
}
