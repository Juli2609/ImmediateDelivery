using ImmediateDelivery.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Data
{
    public class DataContext : IdentityDbContext<User>

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<State> States  { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<State>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name","StateId").IsUnique();
            modelBuilder.Entity<Neighborhood>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<Package>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<PackageType>().HasIndex("Description", "PackageId").IsUnique();
        }
        }
}
