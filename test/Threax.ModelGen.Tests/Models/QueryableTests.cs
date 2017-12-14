using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tests;
using Threax.ModelGen.TestGenerators;
using Xunit;

namespace Threax.ModelGen.Tests.Models
{
    public class QueryableDefaultTests : ModelTests<QueryableDefaultTests.Value>
    {
        public class Value
        {
            [Queryable]
            public String Info { get; set; }
        }
    }

    //public class QueryableRequiredTests : ModelTests<QueryableRequiredTests.Value>
    //{
    //    public class Value
    //    {
    //        [Queryable(required: true)]
    //        public String Info { get; set; }
    //    }
    //}

    //public class QueryableNoUiTests : ModelTests<QueryableNoUiTests.Value>
    //{
    //    public class Value
    //    {
    //        [Queryable(showOnUi: false)]
    //        public String Info { get; set; }
    //    }
    //}

    //public class QueryableInverseDefaultTests : ModelTests<QueryableInverseDefaultTests.Value>
    //{
    //    public class Value
    //    {
    //        [Queryable(showOnUi: false, required: true)]
    //        public String Info { get; set; }
    //    }
    //}
}
