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

namespace Threax.ModelGen.Tests.Models.Relationships.OneToMany
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.OneToMany)]
    public class Left
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.OneToMany)]
    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.Relationships.ManyToOne
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToOne)]
    public class Left
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToOne)]
    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.Relationships.OneToOne
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.OneToOne)]
    public class Left
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.OneToOne)]
    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.Relationships.ManyToMany
{
    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    public class Left
    {
        public String Info { get; set; }
    }

    [RelatedTo(typeof(Left), typeof(Right), RelationKind.ManyToMany)]
    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models
{
    public class RelationshipTestOneToManyLeft : ModelTests<Relationships.OneToMany.Left>
    {
       
    }

    public class RelationshipTestOneToManyRight : ModelTests<Relationships.OneToMany.Right>
    {
        
    }

    public class RelationshipTestManyToOneLeft : ModelTests<Relationships.ManyToOne.Left>
    {

    }

    public class RelationshipTestManyToOneRight : ModelTests<Relationships.ManyToOne.Right>
    {

    }

    public class RelationshipTestOneToOneLeft : ModelTests<Relationships.OneToOne.Left>
    {

    }

    public class RelationshipTestOneToOneRight : ModelTests<Relationships.OneToOne.Right>
    {

    }

    public class RelationshipTestManyToManyLeft : ModelTests<Relationships.ManyToMany.Left>
    {

    }

    public class RelationshipTestManyToManyRight : ModelTests<Relationships.ManyToMany.Right>
    {

    }
}
