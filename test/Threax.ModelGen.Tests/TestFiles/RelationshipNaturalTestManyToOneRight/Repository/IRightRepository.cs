using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InputModels;
using Test.ViewModels;
using Test.Models;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial interface IRightRepository
    {
        Task<Right> Add(RightInput value);
        Task AddRange(IEnumerable<RightInput> values);
        Task Delete(Guid id);
        Task<Right> Get(Guid rightId);
        Task<bool> HasRights();
        Task<RightCollection> List(RightQuery query);
        Task<Right> Update(Guid rightId, RightInput value);
    }
}