using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.Tests.Models.RelationshipMultiple.ManyToMany
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    [RelatedTo(typeof(Left), typeof(Other), RelationKind.ManyToMany)]
    public class Left
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    public class Right
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Other), RelationKind.ManyToMany)]
    public class Other
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models
{
    public class RelationshipMultipleTests : ModelTests<RelationshipMultiple.ManyToMany.Left, RelationshipMultiple.ManyToMany.Right, RelationshipMultiple.ManyToMany.Other>
    {
    }
}
