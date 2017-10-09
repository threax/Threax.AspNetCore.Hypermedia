using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class RepoInterfaceGenerator
    {
        public static String Get(String ns, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, model, Models, models);
        }

        private static String Create(String ns, String Model, String model, String Models, String models) {
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
        Task Delete(Guid id);
        Task<{Model}> Get(Guid {model}Id);
        Task<bool> Has{Models}();
        Task<{Model}Collection> List({Model}Query query);
        Task<{Model}> Update(Guid {model}Id, {Model}Input value);
    }}
}}";
        }
    }
}
