using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Disunity.Store.Data;
using Disunity.Store.Entities;
using Disunity.Store.Pages.Mods;
using Disunity.Store.Policies;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Areas.API.v1.Orgs {

    [Bind("Name", "PrimaryKey", "Value")]
    public class OrgInput {

        public string name { get; set; }
        public string primaryKey { get; set; }
        public string value { get; set; }

    }

    [ApiController]
    [Route("api/v{version:apiVersion}/orgs/{orgSlug:slug}")]
    public class OrgController : ControllerBase {

        private readonly ILogger<Upload> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;

        public OrgController(ILogger<Upload> logger,
                             ApplicationDbContext context,
                             IAuthorizationService authorization,
                             UserManager<UserIdentity> userManager,
                             IMapper mapper) {
            _logger = logger;
            _context = context;
            _authorization = authorization;
            _userManager = userManager;
            _mapper = mapper;
        }

        [FromBody] public OrgInput SubmitModel { get; set; }

        [HttpPost]
        [OrgOperation(Operation.Update,"orgSlug")]
        public async Task<ActionResult<OrgDto>> PostAsync(string orgSlug) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            try {
                var org = _context.Orgs.SingleOrDefault(o => o.Slug == orgSlug);

                if (org == null) {
                    return new NotFoundResult();
                }

                using (var reader = new StreamReader(Request.Body)) {
                    var body = reader.ReadToEnd();
                    _logger.LogWarning(body);
                }

                org.DisplayName = SubmitModel.value;

                await _context.SaveChangesAsync();

                return new JsonResult(_mapper.Map<OrgDto>(org));
            }
            catch (Exception e) {
                var Type = e.GetType().Name;
                var Errors = new object[] { };

//                if (e is ManifestSchemaException schemaExc) {
//                    Errors = schemaExc.Errors.Select(FormatSchemaError).ToArray();
//                } else if (e is InvalidArchiveError formFileError) {
//                    Errors = new[] {formFileError.Message};
//                } else if (e is ArchiveLoadError archiveExc) {
//                    Errors = new[] {archiveExc.Message};
//                } else {
//                    _logger.LogError(e, "");
//                }

                return BadRequest(new {Type, Errors});
            }
        }

    }

}