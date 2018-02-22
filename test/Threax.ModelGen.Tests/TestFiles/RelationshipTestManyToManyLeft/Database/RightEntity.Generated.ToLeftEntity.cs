using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Database
{
    public partial class RightEntity
    {
        public List<LeftToRightEntity> LeftToRightEntities { get; set; }
    }
}

namespace Test.Database
{
    public partial class LeftToRightEntity
    {
        public Guid LeftId { get; set; }

        public LeftEntity Left { get; set; }
    }
}