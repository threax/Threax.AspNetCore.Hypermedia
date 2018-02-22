using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Database
{
    public partial class LeftEntity
    {
        public Guid RightId { get; set; }

        public RightEntity Right { get; set; }
    }
}