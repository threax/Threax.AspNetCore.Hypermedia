using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InputModels;
using Test.ViewModels;
using Test.Models;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial interface IValueRepository
    {
        Task<Value> Add(ValueInput value);
        Task AddRange(IEnumerable<ValueInput> values);
        Task Delete(Guid id);
        Task<Value> Get(Guid valueId);
        Task<bool> HasLotsaValues();
        Task<ValueCollection> List(ValueQuery query);
        Task<Value> Update(Guid valueId, ValueInput value);
    }
}