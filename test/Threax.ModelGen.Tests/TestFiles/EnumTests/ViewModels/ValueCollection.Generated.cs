using Halcyon.HAL.Attributes;
using Test.Controllers.Api;
using Test.Models;
using Test.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Test.ViewModels
{
    public partial class ValueCollection : PagedCollectionView<Value>, IValueQuery
    {
        private ValueQuery query;

        public Guid? ValueId
        {
            get { return query.ValueId; }
            set { query.ValueId = value; }
        }

        protected override void AddCustomQuery(string rel, QueryStringBuilder queryString)
        {
            if (ValueId != null)
            {
                queryString.AppendItem("valueId", ValueId.ToString());
            }


            OnAddCustomQuery(rel, queryString);

            base.AddCustomQuery(rel, queryString);
        }

        partial void OnAddCustomQuery(String rel, QueryStringBuilder queryString);
    }
}