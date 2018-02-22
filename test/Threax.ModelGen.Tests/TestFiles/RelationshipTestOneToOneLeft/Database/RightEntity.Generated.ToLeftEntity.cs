using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Database
{
    public partial class RightEntity
    {
        public Guid LeftId { get; set; }

        public LeftEntity Left { get; set; }
    }
}