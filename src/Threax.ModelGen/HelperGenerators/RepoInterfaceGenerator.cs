using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class RepoInterfaceGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model, model);
        }

        private static String Create(String ns, String Model, String model) {
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
        Task<bool> Has{Model}s();
        Task<{Model}Collection> List({Model}Query query);
        Task<{Model}> Update(Guid {model}Id, {Model}Input value);
    }}
}}";
        }
    }
}
