using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class ControllerGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            String ModelId, modelId;
            NameGenerator.CreatePascalAndCamel(schema.GetKeyName(), out ModelId, out modelId);

            var additionalAuthorize = "";
            String authName = schema.GetAuthorizationRoleString();
            if(authName != null)
            {
                additionalAuthorize = $", Roles = {authName}";
            }
            return Create(ns, Model, model, Models, models, additionalAuthorize, schema.GetKeyType().Name, ModelId, modelId);
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String additionalAuthorize, String modelIdType, String ModelId, String modelId)
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
using {ns}.Models;
using Microsoft.AspNetCore.Authorization;

namespace {ns}.Controllers.Api
{{
    [Route(""api/[controller]"")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer{additionalAuthorize})]
    public partial class {Models}Controller : Controller
    {{
        private I{Model}Repository repo;

        public {Models}Controller(I{Model}Repository repo)
        {{
            this.repo = repo;
        }}

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<{Model}Collection> List([FromQuery] {Model}Query query)
        {{
            return await repo.List(query);
        }}

        [HttpGet(""{{{ModelId}}}"")]
        [HalRel(CrudRels.Get)]
        public async Task<{Model}> Get({modelIdType} {modelId})
        {{
            return await repo.Get({modelId});
        }}

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate(""Cannot add new {model}"")]
        public async Task<{Model}> Add([FromBody]{Model}Input {model})
        {{
            return await repo.Add({model});
        }}

        [HttpPut(""{{{ModelId}}}"")]
        [HalRel(CrudRels.Update)]
        [AutoValidate(""Cannot update {model}"")]
        public async Task<{Model}> Update({modelIdType} {modelId}, [FromBody]{Model}Input {model})
        {{
            return await repo.Update({modelId}, {model});
        }}

        [HttpDelete(""{{{ModelId}}}"")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete({modelIdType} {modelId})
        {{
            await repo.Delete({modelId});
        }}
    }}
}}";
        }
    }
}
