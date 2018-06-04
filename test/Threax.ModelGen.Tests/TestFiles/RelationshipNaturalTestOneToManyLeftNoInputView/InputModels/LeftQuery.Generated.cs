using Halcyon.HAL.Attributes;
using Test.Controllers;
using Test.Models;
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
    public partial class LeftQuery : PagedCollectionQuery, ILeftQuery
    {
        /// <summary>
        /// Lookup a left by id.
        /// </summary>
        public Guid? LeftId { get; set; }


        /// <summary>
        /// Populate an IQueryable for lefts. Does not apply the skip or limit. Will return
        /// true if the query should be modified or false if the entire query was built and should
        /// be left alone.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>True if the query should continue to be built, false if it should be left alone.</returns>
        protected bool CreateGenerated(ref IQueryable<LeftEntity> query)
        {
            if (LeftId != null)
            {
                query = query.Where(i => i.LeftId == LeftId);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}