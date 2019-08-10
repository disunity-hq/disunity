using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;
using Disunity.Store.Entities.Factories;

using Microsoft.AspNetCore.Hosting;

using TopoSort;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(UnityVersionSeed))]
    public class DisunityVersionSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IVersionNumberFactory _versionNumberFactory;

        public DisunityVersionSeed(ApplicationDbContext context, IHostingEnvironment env,
                                   IVersionNumberFactory versionNumberFactory) {
            _context = context;
            _env = env;
            _versionNumberFactory = versionNumberFactory;
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !_context.DisunityVersions.Any();
        }

        public async Task Seed() {
            var versionNumber = await _versionNumberFactory.FindOrCreateVersionNumber("1.0.0");

            var disunityVersion = new DisunityVersion {
                Url =
                    $"https://github.com/disunity-hq/core/releases/download/{versionNumber}/disunity-{versionNumber}.zip",
                VersionNumber = versionNumber,
                CompatibleUnityVersion = new DisunityVersionCompatibility()
            };

            _context.DisunityVersions.Add(disunityVersion);

            await _context.SaveChangesAsync();
        }

    }

}