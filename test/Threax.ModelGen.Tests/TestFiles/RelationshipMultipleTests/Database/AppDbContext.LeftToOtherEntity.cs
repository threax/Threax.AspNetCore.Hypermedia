using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<JoinLeftToOtherEntity> JoinLeftToOther { get; set; }
    }
}
