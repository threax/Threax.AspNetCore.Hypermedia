using System;
using Test.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace Test.InputModels
{
    public partial class LeftQuery
    {
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See LeftQuery.Generated for the generated code

        /// <summary>
        /// Populate an IQueryable. Does not apply the skip or limit.
        /// </summary>
        /// <param name="query">The query to populate.</param>
        /// <returns>The query passed in populated with additional conditions.</returns>
        public Task<IQueryable<LeftEntity>> Create(IQueryable<LeftEntity> query)
        {
            if(CreateGenerated(ref query))
            {
                //Customize query further
            }

            return Task.FromResult(query);
        }
    }
}