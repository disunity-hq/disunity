using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data.Services;
using Disunity.Store.Entities;
using Disunity.Store.Entities.Factories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TopoSort;

using Tracery;

using EnumerableExtensions = Microsoft.EntityFrameworkCore.Internal.EnumerableExtensions;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(TargetSeed), typeof(UserRoleSeed), typeof(DisunityVersionSeed))]
    public class ModSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModSeed> _logger;
        private readonly IHostingEnvironment _env;
        private readonly IVersionNumberFactory _versionNumberFactory;
        private readonly Unparser _unparser;
        private IconRandomizer _iconRandomizer;
        private readonly ISlugifier _slugifier;


        public ModSeed(ApplicationDbContext context,
                       ILogger<ModSeed> logger,
                       IHostingEnvironment env,
                       IVersionNumberFactory versionNumberFactory,
                       Func<string, Unparser> unparserFactory,
                       IconRandomizer iconRandomizer,
                       ISlugifier slugifier) {
            _context = context;
            _logger = logger;
            _env = env;
            _versionNumberFactory = versionNumberFactory;
            _iconRandomizer = iconRandomizer;
            _slugifier = slugifier;

            _unparser = unparserFactory("Entities/mod.json");
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !EnumerableExtensions.Any(_context.Mods);
        }

        public async Task Seed() {
            var random = new Random();
            var orgs = _context.Orgs.ToList();
            var targets = await _context.Targets.ToListAsync();
            var names = new HashSet<string>();
            var disunityVersion = await _context.DisunityVersions.FirstOrDefaultAsync();

            for (var i = 0; i < 45; i++) {
                var org = orgs.PickRandom();
                var target = targets.PickRandom();
                var version = new VersionNumber(random.Next(3), random.Next(3), random.Next(3));
                var attachedVersion = await _versionNumberFactory.FindOrCreateVersionNumber(version);

                var displayName = _unparser.Generate("#display_name.title#");

                while (names.Contains(displayName)) {
                    displayName = _unparser.Generate("#display_name.title#");
                }

                names.Add(displayName);

                var slug = _slugifier.Slugify(displayName);
                var description = _unparser.Generate("#description.capitalize#");
                var iconUrl = _iconRandomizer.GetIconUrl();
                var website = _unparser.Generate("http://#adjective##noun.s#.#tld#");
                var readme = displayName + "\n=====\n\n" + _unparser.Generate("#readme#");

                var modVersion = new ModVersion() {
                    Description = description,
                    Readme = readme,
                    DisplayName = displayName,
                    FileId = "",
                    IconUrl = iconUrl,
                    VersionNumber = attachedVersion,
                    WebsiteUrl = website,
                    IsActive = true,
                    TargetCompatibilities = new List<ModTargetCompatibility>
                        {new ModTargetCompatibility() {Target = target}},
                    DisunityCompatibilities = new List<ModDisunityCompatibility> {
                        new ModDisunityCompatibility() {MinCompatibleVersion = disunityVersion}
                    }
                };

                var mod = new Mod() {
                    Owner = org,
                    Slug = slug,
                    Versions = new List<ModVersion>() {modVersion},
                };

                _context.Mods.Add(mod);
            }

            await _context.SaveChangesAsync();

        }

    }

}