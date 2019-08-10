using System;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    public class UserRoleSeed:ISeeder {

        private readonly ILogger<UserRoleSeed> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public UserRoleSeed(ILogger<UserRoleSeed> logger,
                            RoleManager<IdentityRole> roleManager,
                            ApplicationDbContext dbContext) {

            _logger = logger;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public bool ShouldSeed() {
            return _dbContext.UserRoles.Count() != Enum.GetValues(typeof(UserRoles)).Length;
        }

        public async Task Seed() {
            var roleNames = Enum.GetNames(typeof(UserRoles));

            foreach (var roleName in roleNames) {

                if (await _roleManager.RoleExistsAsync(roleName)) {
                    continue;
                }

                await _roleManager.CreateAsync(new IdentityRole(roleName));
                _logger.LogInformation($"Created role: {roleName}");
            }

        }

    }

}