using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Threax.AspNetCore.Models;
using Test.Models;

namespace Test.Database 
{
    public partial class ValueEntity : IValue, IValueId, ICreatedModified
    {
        [Key]
        public Guid ValueId { get; set; }

        public bool Checkbox { get; set; }

        public DateTime DateOnly { get; set; }

        public String Hidden { get; set; }

        public String Password { get; set; }

        public String Select { get; set; }

        public String TextArea { get; set; }

        public String CustomType { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
