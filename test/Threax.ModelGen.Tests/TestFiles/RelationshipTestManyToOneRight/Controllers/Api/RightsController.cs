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
    public partial class RightsController : Controller
    {
        private IRightRepository repo;

        public RightsController(IRightRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<RightCollection> List([FromQuery] RightQuery query)
        {
            return await repo.List(query);
        }

        [HttpGet("{RightId}")]
        [HalRel(CrudRels.Get)]
        public async Task<Right> Get(Guid rightId)
        {
            return await repo.Get(rightId);
        }

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate("Cannot add new right")]
        public async Task<Right> Add([FromBody]RightInput right)
        {
            return await repo.Add(right);
        }

        [HttpPut("{RightId}")]
        [HalRel(CrudRels.Update)]
        [AutoValidate("Cannot update right")]
        public async Task<Right> Update(Guid rightId, [FromBody]RightInput right)
        {
            return await repo.Update(rightId, right);
        }

        [HttpDelete("{RightId}")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid rightId)
        {
            await repo.Delete(rightId);
        }
    }
}