using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class ControllerGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model, model);
        }

        private static String Create(String ns, String Model, String model)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using {ns}.Repository;
using Threax.AspNetCore.Halcyon.Ext;
using {ns}.ViewModels;
using {ns}.InputModels;

namespace {ns}.Controllers.Api
{{
    [Route(""api/[controller]"")]
    [ResponseCache(NoStore = true)]
    //[Authorize(Roles = Roles.Edit{Model}s, ActiveAuthenticationSchemes = AuthSchemes.Bearer)] //Uncomment this to secure the api, you will probably have to define the role
    public partial class {Model}sController : Controller
    {{
        private I{Model}Repository repo;

        public {Model}sController(I{Model}Repository repo)
        {{
            this.repo = repo;
        }}

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<{Model}Collection> List([FromQuery] {Model}Query query)
        {{
            return await repo.List(query);
        }}

        [HttpGet(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Get)]
        public async Task<{Model}> Get(Guid {model}Id)
        {{
            return await repo.Get({model}Id);
        }}

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate(""Cannot add new {model}"")]
        public async Task<{Model}> Add([FromBody]{Model}Input {model})
        {{
            return await repo.Add({model});
        }}

        [HttpPut(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Update)]
        [AutoValidate(""Cannot update {model}"")]
        public async Task<{Model}> Update(Guid {model}Id, [FromBody]{Model}Input {model})
        {{
            return await repo.Update({model}Id, {model});
        }}

        [HttpDelete(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid {model}Id)
        {{
            await repo.Delete({model}Id);
        }}
    }}
}}";
        }
    }
}
