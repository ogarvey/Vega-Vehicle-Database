using Microsoft.EntityFrameworkCore;
using vega.Data.Models;

namespace vega.Data.Persistence
{
    public class VegaDbContext : DbContext
    {
        public VegaDbContext(DbContextOptions<VegaDbContext> options)
 : base(options)
        {

        }

        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VehicleFeature>()
                .HasKey(vf => new {vf.VehicleId, vf.FeatureId});
        }
    }
}