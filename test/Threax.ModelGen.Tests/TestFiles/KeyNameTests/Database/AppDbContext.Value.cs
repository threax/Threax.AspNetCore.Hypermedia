using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<ValueEntity> Values { get; set; }
    }
}
