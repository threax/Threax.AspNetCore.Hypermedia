using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Database
{
    public partial class JoinLeftToOtherEntity
    {
        public Guid LeftId { get; set; }

        public LeftEntity Left { get; set; }

        public Guid OtherId { get; set; }

        public OtherEntity Other { get; set; }
    }
}