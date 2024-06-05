using Common.Extensions;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Interfaces;
//using Profile.Models;
using System.Security.Claims;

namespace Profile.Controllers
{
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfileService _profileService;
        private IBus _bus;

        public ProfileController(IProfileService profileService, IBus bus)
        {
            _profileService = profileService;
            _bus = bus;
        }

        [HttpGet]
        [Authorize]
        [Route(template: "/profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _profileService.GetProfile(Guid.Parse(User.FindFirstValue("id")));

            if (!profile.Success)
                return NotFound(profile.CastToBase());

            return Ok(profile.Load);
        }

        [HttpPut]
        [Authorize]
        [Route(template: "/profile")]
        public async Task<IActionResult> EditProfile(ProfileEditModel model)
        {
            bool isClosed = _bus.GetApplicationStatus(Guid.Parse(User.FindFirstValue("id")));
            if (isClosed)
                return Forbid();

            var result = await _profileService.EditProfile(model, Guid.Parse(User.FindFirstValue("id")), true);

            if (!result.Success)
                return BadRequest(result.CastToErrors());

            return Ok(result.CastToBase());
        }
    }
}
