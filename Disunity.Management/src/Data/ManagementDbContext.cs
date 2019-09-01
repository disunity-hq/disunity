using System.Reflection;

using Disunity.Management.Models;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Management.Data {

    public class ManagementDbContext : DbContext {

        public ManagementDbContext() { }

        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options) { }

        public DbSet<TargetMeta> Targets { get; set; }
        public DbSet<ProfileMeta> Profiles { get; set; }
        public DbSet<TargetProfile> TargetProfiles { get; set; }
        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder.UseSqlite("Data Source=disunity.db");
        }

    }

}