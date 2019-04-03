using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InputModels;
using Test.ViewModels;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.Repository
{
    public partial interface IValueRepository
    {
        Task<Value> Add(ValueInput value);
        Task AddRange(IEnumerable<ValueInput> values);
        Task Delete(String id);
        Task<Value> Get(String valueId);
        Task<bool> HasValues();
        Task<ValueCollection> List(ValueQuery query);
        Task<Value> Update(String valueId, ValueInput value);
    }
}