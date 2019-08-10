using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Pages.Orgs {

    [Authorize]
    public class Create : PageModel {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ILogger<Create> _logger;

        [FromForm(Name = "displayName")] public string DisplayName { get; set; }

        public Create(ApplicationDbContext dbContext, UserManager<UserIdentity> userManager, ILogger<Create> logger) {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<IActionResult> OnPostAsync() {
            try {
                var owner = await _userManager.GetUserAsync(User);

                var org = new Org {
                    DisplayName = DisplayName,
                    Members = new List<OrgMember> {
                        new OrgMember {
                            Role = OrgMemberRole.Owner,
                            User = owner
                        }
                    }
                };

                _dbContext.Orgs.Add(org);

                await _dbContext.SaveChangesAsync();

                return RedirectToPage("/Orgs/Details", new {
                    orgSlug = org.Slug
                });
            }
            catch (Exception e) {
                _logger.LogError(e, "Error creating org");
                ModelState.AddModelError("UnkownError", "An error occurred creating the org");
                return Page();
            }

        }

    }

}