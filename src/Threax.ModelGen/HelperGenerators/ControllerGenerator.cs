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
    [Route(""[controller]"")]
    [ResponseCache(NoStore = true)]
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
            Task task = null;
            OnList(query, ref task);
            if(task != null)
            {{
                await task;
            }}
            return await repo.List(query);
        }}

        partial void OnList({Model}Query query, ref Task task);

        [HttpGet(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Get)]
        public async Task<{Model}> Get(Guid {model}Id)
        {{
            Task task = null;
            OnGet({model}Id, ref task);
            if (task != null)
            {{
                await task;
            }}
            return await repo.Get({model}Id);
        }}

        partial void OnGet(Guid {model}Id, ref Task task);

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate(""Cannot add new {model}"")]
        public async Task<{Model}> Add([FromBody]{Model}Input {model})
        {{
            Task task = null;
            OnAdd({model}, ref task);
            if (task != null)
            {{
                await task;
            }}
            return await repo.Add({model});
        }}

        partial void OnAdd({Model}Input {model}, ref Task task);

        [HttpPut(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Update)]
        [AutoValidate(""Cannot update {model}"")]
        public async Task<{Model}> Update(Guid {model}Id, [FromBody]{Model}Input {model})
        {{
            Task task = null;
            OnUpdate({model}, ref task);
            if (task != null)
            {{
                await task;
            }}
            return await repo.Update({model}Id, {model});
        }}

        partial void OnUpdate({Model}Input {model}, ref Task task);

        [HttpDelete(""{{{Model}Id}}"")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid {model}Id)
        {{
            Task task = null;
            OnDelete({model}Id, ref task);
            if (task != null)
            {{
                await task;
            }}
            await repo.Delete({model}Id);
        }}

        partial void OnDelete(Guid {model}Id, ref Task task);
    }}
}}";
        }
    }
}
