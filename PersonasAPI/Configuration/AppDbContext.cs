using Microsoft.EntityFrameworkCore;
using PersonasAPI.Entities;

namespace PersonasAPI.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext()
        {
            Database.SetCommandTimeout(0); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Personas> Personas { get; set; }
    }    
}
