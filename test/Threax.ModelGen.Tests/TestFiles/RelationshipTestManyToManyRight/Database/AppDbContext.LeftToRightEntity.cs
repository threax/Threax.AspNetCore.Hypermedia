using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<JoinLeftToRightEntity> JoinLeftToRight { get; set; }
    }
}
