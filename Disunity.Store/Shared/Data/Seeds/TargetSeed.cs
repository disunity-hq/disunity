using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data.Services;
using Disunity.Store.Entities;
using Disunity.Store.Entities.Factories;
using Disunity.Store.Extensions;

using Microsoft.AspNetCore.Hosting;

using TopoSort;

using Tracery;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(UnityVersionSeed))]
    public class TargetSeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IVersionNumberFactory _versionNumberFactory;
        private readonly ISlugifier _slugifier;
        private readonly IconRandomizer _iconRandomizer;
        private readonly Unparser _unparser;

        public TargetSeed(ApplicationDbContext context, IHostingEnvironment env,
                          IVersionNumberFactory versionNumberFactory,
                          ISlugifier slugifier,
                          Func<string, Unparser> unparserFactory,
                          IconRandomizer iconRandomizer) {
            _context = context;
            _env = env;
            _versionNumberFactory = versionNumberFactory;
            _slugifier = slugifier;
            _iconRandomizer = iconRandomizer;
            _unparser = unparserFactory("Entities/target.json");
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !_context.Targets.Any();
        }

        public Task Seed() {
            var unity2018Min =
                _context.UnityVersions.FindExactVersion((VersionNumber) "2018.1.0").Single();

            var unity2018Max =
                _context.UnityVersions.FindExactVersion((VersionNumber) "2018.4.4").Single();

            for (var i = 0; i < 10; i++) {
                var displayName = _unparser.Generate("#display_name.title#");
                var slug = _slugifier.Slugify(displayName);
                var iconUrl = _iconRandomizer.GetIconUrl();
                var description = _unparser.Generate("#description#");
                
                var targetVersion = new TargetVersion() {
                    Description = description,
                    Hash = $"0123456789abcdef-{i}",
                    DisplayName = displayName,
                    IconUrl = iconUrl,
                    WebsiteUrl = "",
                    VersionNumber = "1",
                    DisunityCompatibility = new TargetVersionCompatibility() {
                        MinCompatibleVersion = unity2018Min,
                        MaxCompatibleVersion = unity2018Max
                    }
                };

                var target = new Target() {
                    Slug = slug,
                    Versions = new List<TargetVersion>() {
                        targetVersion
                    }
                };

                _context.Targets.Add(target);
            }


            return _context.SaveChangesAsync();
        }

    }

}