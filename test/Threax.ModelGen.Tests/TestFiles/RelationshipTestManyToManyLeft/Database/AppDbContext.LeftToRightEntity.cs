using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<LeftToRightEntity> JoinLeftToRight { get; set; }
    }
}
