using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Database
{
    public partial class JoinLeftToRightEntity
    {
        public Guid RightId { get; set; }

        public RightEntity Right { get; set; }

        public Guid LeftId { get; set; }

        public LeftEntity Left { get; set; }
    }
}