using Microsoft.EntityFrameworkCore;

namespace Test.Database
{
    public partial class AppDbContext
    {
        public DbSet<ValueEntity> LotsaValues { get; set; }
    }
}
