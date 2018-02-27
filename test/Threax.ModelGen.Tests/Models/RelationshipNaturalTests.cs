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

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.OneToMany
{
    public class Left
    {
        public String Info { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        public List<Left> Lefts { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.ManyToOne
{
    public class Left
    {
        public String Info { get; set; }

        public List<Right> Rights { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.OneToOne
{
    public class Left
    {
        public String Info { get; set; }

        public Right Right { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        public Left Left { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.ManyToMany
{
    public class Left
    {
        public String Info { get; set; }

        public List<Right> Rights { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        public List<Left> Lefts { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models
{
    //public class RelationshipTestOneToManyLeft : ModelTests<RelationshipNatural.OneToMany.Left, RelationshipNatural.OneToMany.Right>
    //{
       
    //}

    //public class RelationshipTestOneToManyRight : ModelTests<RelationshipNatural.OneToMany.Right, RelationshipNatural.OneToMany.Left>
    //{
        
    //}

    //public class RelationshipTestManyToOneLeft : ModelTests<RelationshipNatural.ManyToOne.Left, RelationshipNatural.ManyToOne.Right>
    //{

    //}

    //public class RelationshipTestManyToOneRight : ModelTests<RelationshipNatural.ManyToOne.Right, RelationshipNatural.ManyToOne.Left>
    //{

    //}

    //public class RelationshipTestOneToOneLeft : ModelTests<RelationshipNatural.OneToOne.Left, RelationshipNatural.OneToOne.Right>
    //{

    //}

    //public class RelationshipTestOneToOneRight : ModelTests<RelationshipNatural.OneToOne.Right, RelationshipNatural.OneToOne.Left>
    //{

    //}

    //public class RelaationshipTestManyToManyLeft : ModelTests<RelationshipNatural.ManyToMany.Left, RelationshipNatural.ManyToMany.Right>
    //{

    //}

    //public class RelationshipTestManyToManyRight : ModelTests<RelationshipNatural.ManyToMany.Right, RelationshipNatural.ManyToMany.Left>
    //{

    //}
}
