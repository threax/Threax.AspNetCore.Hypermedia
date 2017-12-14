using Halcyon.HAL.Attributes;
using Test.Controllers;
using Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace Test.InputModels
{
    public abstract  class ValueQueryBase : PagedCollectionQuery
    {
        [UiOrder]
        [UiSearch]
        public abstract int? Num { get; set; }

    }


    [HalModel]
    public partial class ValueQuery : ValueQueryBase, IValueQuery
    {
        /// <summary>
        /// Lookup a value by id.
        /// </summary>
        public Guid? ValueId { get; set; }


        /// <summary>
        /// Populate an IQueryable for values. Does not apply the skip or limit.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public IQueryable<T> Create<T>(IQueryable<T> query) where T : IValue, IValueId
        {
            if (ValueId != null)
            {
                query = query.Where(i => i.ValueId == ValueId);
            }
            else
            {
                if (Num != null)
                {
                    query = query.Where(i => i.Num == Num);
                }

                OnCreate(ref query);
            }

            return query;
        }

        partial void OnCreate<T>(ref IQueryable<T> query) where T : IValue, IValueId;
    }
}