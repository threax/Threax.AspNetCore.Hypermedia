using Threax.NJsonSchema;
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

        public Right Right { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        public List<Left> Lefts { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.OneToMany.WithNoInputViewModel
{
    public class Left
    {
        public String Info { get; set; }

        [NoInputModel]
        [NoViewModel]
        public Right Right { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        [NoInputModel]
        [NoViewModel]
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

        public Left Left { get; set; }
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

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.WithValueProvider
{
    public class RightValueProvider //Doesnt really have to define interface for tests
    {

    }

    public class Left
    {
        public String Info { get; set; }

        [DefineValueProvider(typeof(RightValueProvider))]
        public List<Right> Rights { get; set; }
    }

    public class Right
    {
        public String Info { get; set; }

        public List<Left> Lefts { get; set; }
    }
}

namespace Threax.ModelGen.Tests.Models.RelationshipNatural.ManyToMany.WithDisplayName
{
    public class Left
    {
        public String Info { get; set; }

        [Display(Name = "The Rights")]
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
    public class RelationshipNaturalTestOneToManyLeft : ModelTests<RelationshipNatural.OneToMany.Left, RelationshipNatural.OneToMany.Right>
    {

    }

    public class RelationshipNaturalTestOneToManyRight : ModelTests<RelationshipNatural.OneToMany.Right, RelationshipNatural.OneToMany.Left>
    {

    }

    public class RelationshipNaturalTestOneToManyLeftNoInputView : ModelTests<RelationshipNatural.OneToMany.WithNoInputViewModel.Left, RelationshipNatural.OneToMany.WithNoInputViewModel.Right>
    {

    }

    public class RelationshipNaturalTestOneToManyRightNoInputView : ModelTests<RelationshipNatural.OneToMany.WithNoInputViewModel.Right, RelationshipNatural.OneToMany.WithNoInputViewModel.Left>
    {

    }

    public class RelationshipNaturalTestManyToOneLeft : ModelTests<RelationshipNatural.ManyToOne.Left, RelationshipNatural.ManyToOne.Right>
    {

    }

    public class RelationshipNaturalTestManyToOneRight : ModelTests<RelationshipNatural.ManyToOne.Right, RelationshipNatural.ManyToOne.Left>
    {

    }

    public class RelationshipNaturalTestOneToOneLeft : ModelTests<RelationshipNatural.OneToOne.Left, RelationshipNatural.OneToOne.Right>
    {

    }

    public class RelationshipNaturalTestOneToOneRight : ModelTests<RelationshipNatural.OneToOne.Right, RelationshipNatural.OneToOne.Left>
    {

    }

    public class RelationshipNaturalTestManyToManyLeft : ModelTests<RelationshipNatural.ManyToMany.Left, RelationshipNatural.ManyToMany.Right>
    {

    }

    public class RelationshipNaturalTestManyToManyRight : ModelTests<RelationshipNatural.ManyToMany.Right, RelationshipNatural.ManyToMany.Left>
    {

    }

    public class RelationshipNaturalTestManyToManyWithValueProviderLeft : ModelTests<RelationshipNatural.WithValueProvider.Left, RelationshipNatural.WithValueProvider.Right>
    {

    }

    public class RelationshipNaturalTestManyToManyWithValueProviderRight : ModelTests<RelationshipNatural.WithValueProvider.Right, RelationshipNatural.WithValueProvider.Left>
    {

    }

    public class RelationshipNaturalTestManyToManyWithDisplayNameLeft : ModelTests<RelationshipNatural.ManyToMany.WithDisplayName.Left, RelationshipNatural.ManyToMany.WithDisplayName.Right>
    {

    }

    public class RelationshipNaturalTestManyToManyWithDisplayNameRight : ModelTests<RelationshipNatural.ManyToMany.WithDisplayName.Right, RelationshipNatural.ManyToMany.WithDisplayName.Left>
    {

    }
}
