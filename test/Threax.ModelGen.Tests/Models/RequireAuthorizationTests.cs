using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tests;
using Threax.ModelGen.TestGenerators;
using Xunit;

namespace Threax.ModelGen.Tests.Models
{
    public class RequireAuthorizationTests : ModelTests<RequireAuthorizationTests.Value>
    {
        [RequireAuthorization(typeof(Roles), nameof(Roles.Administrator))]
        public class Value
        {

        }

        public class Roles
        {
            public const String Administrator = "Admin";
        }
    }
}
