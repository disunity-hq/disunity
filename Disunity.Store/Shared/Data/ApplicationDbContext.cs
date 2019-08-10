using System.Data;
using System.Reflection;

using Disunity.Store.Entities;

using EFCoreHooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Npgsql;


namespace Disunity.Store.Data {

    public class ApplicationDbContext : HookedIdentityDbContext<UserIdentity> {

        private readonly ILogger<ApplicationDbContext> _logger;

        static ApplicationDbContext() {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ModDependencyType>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    HookManagerContainer hooks,
                                    ILogger<ApplicationDbContext> logger) : base(options, hooks) {

            _logger = logger;

            Database.GetDbConnection().StateChange += (sender, args) => {
                if (args.CurrentState == ConnectionState.Open) {
                    (Database.GetDbConnection() as NpgsqlConnection)?.ReloadTypes();
                }
            };
        }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModDependency> ModDependencies { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }
        public DbSet<ModTargetCompatibility> ModTargetCompatibilities { get; set; }
        public DbSet<ModDisunityCompatibility> ModDisunityCompatibilities { get; set; }

        public DbSet<UnityVersion> UnityVersions { get; set; }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetVersion> TargetVersions { get; set; }
        public DbSet<TargetVersionCompatibility> TargetVersionCompatibilities { get; set; }

        public DbSet<DisunityVersion> DisunityVersions { get; set; }
        public DbSet<DisunityVersionCompatibility> DisunityVersionCompatibilities { get; set; }

        public DbSet<VersionNumber> VersionNumbers { get; set; }

        public DbSet<StoredFile> StoredFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ForNpgsqlHasEnum<OrgMemberRole>();
            builder.ForNpgsqlHasEnum<ModDependencyType>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }

}