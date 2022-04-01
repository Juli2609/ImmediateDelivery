using ImmediateDelivery.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Data
{
    public class DataContext : DbContext    
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
        }
        }
}
