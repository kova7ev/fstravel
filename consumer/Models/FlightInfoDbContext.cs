using Microsoft.EntityFrameworkCore;

namespace FlightInfoConsumer.Models
{
    public class FlightInfoDbContext : DbContext
    {
        public FlightInfoDbContext(DbContextOptions<FlightInfoDbContext> options) : base(options)
        {
        }
        public DbSet<FlightInfoRequest> FlightInfo { get; set; }
    }
}