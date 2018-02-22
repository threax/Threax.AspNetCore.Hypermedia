using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<LeftEntity> Lefts { get; set; }
    }
}
