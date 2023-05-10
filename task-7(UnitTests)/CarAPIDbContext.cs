using Microsoft.EntityFrameworkCore;
using Task7_Programing.Models;

namespace Task7_Programing.Data
{
    public class CarAPIDbContext : DbContext
    {
        public CarAPIDbContext(DbContextOptions<CarAPIDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logger> Loggers { get; set; }
    }

    public class UsersAPIDbContext : DbContext 
    {
        public UsersAPIDbContext(DbContextOptions<UsersAPIDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }

}
