using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;

namespace Test.Database 
{
    public partial class ValueEntity : ICreatedModified
    {
        [Key]
        public Guid ValueId { get; set; }

        public Guid? OptionalId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
