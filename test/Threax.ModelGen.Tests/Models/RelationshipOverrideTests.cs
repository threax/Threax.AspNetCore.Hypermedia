using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.Tests.Models.RelationshipOverrides.ManyToMany
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    public class Left
    {
        public String Info { get; set; }

        [Display(Name = "Cool Right Things")]
        [NoEntity]
        public List<Guid> RightIds { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models
{
    public class RelationshipOverrideManyToManyTests : ModelTests<RelationshipOverrides.ManyToMany.Left, RelationshipOverrides.ManyToMany.Right>
    {
    }
}
