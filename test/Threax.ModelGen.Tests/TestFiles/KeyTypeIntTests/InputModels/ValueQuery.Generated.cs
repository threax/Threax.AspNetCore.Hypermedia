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
    public partial class ValueQuery : PagedCollectionQuery
    {
        /// <summary>
        /// Lookup a value by id.
        /// </summary>
        public Int32? ValueId { get; set; }

        /// <summary>
        /// Populate an IQueryable for values. Does not apply the skip or limit. Will return
        /// true if the query should be modified or false if the entire query was built and should
        /// be left alone.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>True if the query should continue to be built, false if it should be left alone.</returns>
        protected bool CreateGenerated(ref IQueryable<ValueEntity> query)
        {
            if (ValueId != null)
            {
                query = query.Where(i => i.ValueId == ValueId);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}