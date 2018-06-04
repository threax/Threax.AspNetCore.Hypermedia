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
    public partial class LeftsController : Controller
    {
        private ILeftRepository repo;

        public LeftsController(ILeftRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [HalRel(CrudRels.List)]
        public async Task<LeftCollection> List([FromQuery] LeftQuery query)
        {
            return await repo.List(query);
        }

        [HttpGet("{LeftId}")]
        [HalRel(CrudRels.Get)]
        public async Task<Left> Get(Guid leftId)
        {
            return await repo.Get(leftId);
        }

        [HttpPost]
        [HalRel(CrudRels.Add)]
        [AutoValidate("Cannot add new left")]
        public async Task<Left> Add([FromBody]LeftInput left)
        {
            return await repo.Add(left);
        }

        [HttpPut("{LeftId}")]
        [HalRel(CrudRels.Update)]
        [AutoValidate("Cannot update left")]
        public async Task<Left> Update(Guid leftId, [FromBody]LeftInput left)
        {
            return await repo.Update(leftId, left);
        }

        [HttpDelete("{LeftId}")]
        [HalRel(CrudRels.Delete)]
        public async Task Delete(Guid leftId)
        {
            await repo.Delete(leftId);
        }
    }
}