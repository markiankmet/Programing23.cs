using Microsoft.EntityFrameworkCore;
using Task_5_StudyPractik.Models;

namespace Task_5_StudyPractik.Data
{
    public class FlightAPIDbContext : DbContext
    {
        public FlightAPIDbContext(DbContextOptions options): base(options) { }

        public DbSet<Flight> Flights { get; set; }
    }
}
