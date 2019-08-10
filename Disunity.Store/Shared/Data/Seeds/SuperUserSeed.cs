using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using TopoSort;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(UserRoleSeed))]
    public class SuperUserSeed : ISeeder {

        private readonly ILogger<SuperUserSeed> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ApplicationDbContext _dbContext;

        private readonly RoleManager<IdentityRole> _roleManager;


        public SuperUserSeed(IConfiguration config,
                             ILogger<SuperUserSeed> logger,
                             UserManager<UserIdentity> userManager,
                             ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager) {


            _logger = logger;
            _config = config;
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public bool ShouldSeed() {
            // should seed if admin user doesn't exist
            return !_dbContext.Users.Any(u => u.Email == _config["AdminUser:Email"]);
        }

        public async Task Seed() {
            var password = _config["AdminUser:Password"];
            var emailAddress = _config["AdminUser:Email"];

            if (emailAddress == null || password == null) {
                _logger.LogInformation("Skipping creating super user as user was missing email or password");
                return;
            }

            if (await _userManager.FindByEmailAsync(emailAddress) != null) {
                _logger.LogWarning($"Skipping creating super user as email is already taken: {emailAddress}");
                return;
            }

            var superuser = new UserIdentity {
                UserName = _config.GetValue("AdminUser:Name", emailAddress),
                Email = emailAddress
            };

            var createSuperUser = await _userManager.CreateAsync(superuser, password);

            if (!createSuperUser.Succeeded) {
                _logger.LogWarning($"Failed to create admin user: {emailAddress}");
                return;
            }

            _logger.LogInformation($"Successfully created admin user: {superuser.Email}");

            var addedRole = await _userManager.AddToRoleAsync(superuser, UserRoles.Admin.ToString());

            if (addedRole.Succeeded) {
                _logger.LogInformation($"Successfully set admin role: {superuser.Email}");
            }

        }

    }

}