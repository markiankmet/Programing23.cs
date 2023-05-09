using Microsoft.EntityFrameworkCore;
using Task_6_StudyPractik.Models;

namespace Task_6_StudyPractik.Data
{
    public class FlightAPIDbContext : DbContext
    {
        public FlightAPIDbContext(DbContextOptions options): base(options) { }
        
        public DbSet<Flight> Flights { get; set; }
    }
}
