using Halcyon.HAL.Attributes;
using Test.Controllers;
using Test.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace Test.InputModels
{
    [HalModel]
    public partial class RightQuery : PagedCollectionQuery
    {
        /// <summary>
        /// Lookup a right by id.
        /// </summary>
        public Guid? RightId { get; set; }

        /// <summary>
        /// Populate an IQueryable. Does not apply the skip or limit.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public Task<IQueryable<RightEntity>> Create(IQueryable<RightEntity> query)
        {
            if (RightId != null)
            {
                query = query.Where(i => i.RightId == RightId);
            }
            else
            {
                //Customize query further
            }

            return Task.FromResult(query);
        }
    }
}