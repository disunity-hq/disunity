using System.Collections.Generic;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data.Seeds;

using Microsoft.Extensions.Logging;

using TopoSort;


namespace Disunity.Store.Data {

    [AsScoped]
    public class DbSeeder {

        private readonly ILogger<DbSeeder> _logger;

        private readonly IEnumerable<ISeeder> _seeds;

        public DbSeeder(IEnumerable<ISeeder> seeds, ILogger<DbSeeder> logger) {
            _logger = logger;
            _seeds = seeds.TopoSort();
        }

        public async Task Seed() {


            foreach (var seed in _seeds) {
                _logger.LogDebug($"Checking if {seed.GetType().Name} can run");
                if (seed.ShouldSeed()) {
                    _logger.LogInformation($"Running seed {seed.GetType().Name}");
                    await seed.Seed();
                }
            }
        }

    }

}