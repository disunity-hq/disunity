using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Policies {

    [AsScoped(typeof(IAuthorizationHandler))]
    public class ModPolicyHandler : OperationPolicyHandler<Mod> {

        private readonly ILogger<ModPolicyHandler> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ModPolicyHandler(ILogger<ModPolicyHandler> logger, ApplicationDbContext dbContext) {
            _logger = logger;
            _dbContext = dbContext;

        }

        private static bool ReadOp(OrgMember membership) {
            return membership != null;
        }

        private static bool CreateOp(OrgMember membership) {
            return membership != null;
        }

        private static bool UpdateOp(OrgMember membership) {
            return membership != null;
        }

        private static bool DeleteOp(OrgMember membership) {
            return membership != null;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             OperationRequirement requirement, Mod resource) {
            if (CheckSuperUser(context, requirement)) {
                return;
            }

            var methodInfo = GetHandler(requirement);

            if (methodInfo == null) {
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var membership =
                await _dbContext.OrgMembers.SingleOrDefaultAsync(
                    m => m.OrgId == resource.OwnerId && m.User.Id == userId);

            var authorized = (bool) methodInfo.Invoke(null, new object[] {membership});

            if (authorized) {
                context.Succeed(requirement);
            }

        }

    }

}