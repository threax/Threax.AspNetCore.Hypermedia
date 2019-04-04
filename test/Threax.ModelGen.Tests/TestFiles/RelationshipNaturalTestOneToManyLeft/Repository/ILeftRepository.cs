using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InputModels;
using Test.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial interface ILeftRepository
    {
        Task<Left> Add(LeftInput value);
        Task AddRange(IEnumerable<LeftInput> values);
        Task Delete(Guid id);
        Task<Left> Get(Guid leftId);
        Task<bool> HasLefts();
        Task<LeftCollection> List(LeftQuery query);
        Task<Left> Update(Guid leftId, LeftInput value);
    }
}