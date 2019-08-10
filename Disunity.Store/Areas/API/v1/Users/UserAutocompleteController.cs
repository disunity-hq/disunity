using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Data;
using Disunity.Store.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.EJ2.Linq;


namespace Disunity.Store.Areas.API.v1.Users {

    [ApiController]
    [Route("api/v{version:apiVersion}/users/autocomplete")]
    public class UserAutocompleteController : ControllerBase {

        private readonly ILogger<UserAutocompleteController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserAutocompleteController(ILogger<UserAutocompleteController> logger, ApplicationDbContext dbContext) {
            _logger = logger;
            _dbContext = dbContext;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetUserNames([FromQuery] string username) {
            if (username.Length < 3) {
                return new JsonResult(new object[] { });
            }

            var users = await _dbContext.Users
                                        .Where(u => u.UserName.StartsWith(username))
                                        .Select(u => u.UserName)
                                        .ToListAsync();

            return new JsonResult(users);
        }

    }

}