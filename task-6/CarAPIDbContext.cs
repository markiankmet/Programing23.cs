using Microsoft.EntityFrameworkCore;
using Task_6_PR_2._0.Model;

namespace Task_6_PR_2._0.Data
{
    public class CarAPIDbContext: DbContext
    {
        public CarAPIDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logger> Loggers { get; set; }
    }
}
