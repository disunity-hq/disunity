using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data.Services;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;

using TopoSort;

using Tracery;


namespace Disunity.Store.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    [DependsOn(typeof(UserRoleSeed))]
    public class UserIdentitySeed : ISeeder {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserIdentitySeed> _logger;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IHostingEnvironment _env;
        private readonly ISlugifier _slugifier;
        private readonly IconRandomizer _iconRandomizer;
        private readonly Unparser _unparser;

        public UserIdentitySeed(ApplicationDbContext context,
                                ILogger<UserIdentitySeed> logger,
                                IHostingEnvironment env,
                                ISlugifier slugifier,
                                Func<string, Unparser> unparserFactory,
                                IconRandomizer iconRandomizer, 
                                UserManager<UserIdentity> userManager) {
            _context = context;
            _logger = logger;
            _env = env;
            _slugifier = slugifier;
            _iconRandomizer = iconRandomizer;
            _userManager = userManager;
            _unparser = unparserFactory("all.json");
        }


        public bool ShouldSeed() {
            var users = _userManager.GetUsersInRoleAsync(UserRoles.User.ToString()).Result;
            return _env.IsDevelopment() && users.Count() <= 1;
        }

        public async Task Seed() {
            for (var i = 0; i < 100; i++) {
                var displayName = _unparser.Generate("#name.title##name.title##double_digit#");
                var slug = _slugifier.Slugify(displayName);
                var email = _unparser.Generate($"{slug}@#noun#.#tld#");

                var org = new Org() {
                    Slug = slug,
                    DisplayName = displayName
                };

                var user = new UserIdentity() {
                    UserName = slug,
                    Email = email,
                    ShadowOrg = org
                };

                await _userManager.CreateAsync(user, "Password1!");

                var roleAdded = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
                
                if (!roleAdded.Succeeded) {
                    var message = $"Couldn't set user role for {slug}: {roleAdded.Errors.ToList()[0].Description}";
                    throw new DbSeedException(message);
                }

                var membership = new OrgMember() {
                    User = user,
                    Org = org,
                    Role = OrgMemberRole.Owner
                };

                _context.Attach(membership);

                _logger.LogInformation($"Created user: `{displayName}` => {slug}");
            }
            
            await _context.SaveChangesAsync();
        }

    }

}