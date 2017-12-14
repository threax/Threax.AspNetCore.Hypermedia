using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Repository;
using Threax.AspNetCore.Halcyon.Ext;
using Test.ViewModels;
using Test.InputModels;
using Test.Models;
using Microsoft.AspNetCore.Authorization;

namespace Test.Controllers.Api
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    [Authorize(AuthenticationSchemes = AuthCoreSchemes.Bearer)]
    public partial class ValuesController : Controller
    {
        private IValueRepository repo;

        public ValuesController(IValueRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<ValueCollection> List([FromQuery] ValueQuery query)
        {
            return await repo.List(query);
        }

        [HttpGet("{ValueId}")]
        [HalRel(CrudRels.Get)]
        public async Task<Value> Get(Int32 valueId)
        {
            return await repo.Get(valueId);
        }

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate("Cannot add new value")]
        public async Task<Value> Add([FromBody]ValueInput value)
        {
            return await repo.Add(value);
        }

        [HttpPut("{ValueId}")]
        [HalRel(CrudRels.Update)]
        [AutoValidate("Cannot update value")]
        public async Task<Value> Update(Int32 valueId, [FromBody]ValueInput value)
        {
            return await repo.Update(valueId, value);
        }

        [HttpDelete("{ValueId}")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Int32 valueId)
        {
            await repo.Delete(valueId);
        }
    }
}