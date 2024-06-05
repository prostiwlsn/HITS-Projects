using Auth.Interfaces;
using Auth.Models;
using Common;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;

namespace Auth.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //private IBus _bus;
        private IAuthenticationService _authenticationService;
        private ICredentialService _credentialService;
        public AuthenticationController(IAuthenticationService authenticationService, ICredentialService credentialService)
        {
            _authenticationService = authenticationService;
            _credentialService = credentialService;
        }

        [HttpPost]
        [Route(template: "user/register")]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            var result = await _authenticationService.Register(model);

            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.Fail)
                    return BadRequest(result.CastToErrors());
            }

            return Ok(result.CastToBase());
        }

        [HttpPost]
        [Route(template: "user/login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authenticationService.Login(model.Email, model.Password);

            if (!result.Success)
                return BadRequest(result.CastToBase());

            return Ok(result.Load);
        }

        [HttpPost]
        [Route(template: "user/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.Logout();
            return Ok(result);
        }

        [HttpPost]
        [Route(template: "user/token/refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh()
        {
            string type = User.FindFirstValue("type");

            if (type != "refresh")
                return BadRequest(new { Message = "Wrong token type" });

            Guid sessionId = new Guid(User.FindFirstValue("sessionId"));

            var result = await _authenticationService.Refresh(sessionId);

            if (!result.Success) 
                return BadRequest(new { Message = result.Message });

            return Ok(result.Load);
        }

        [HttpPost]
        [Route(template: "user/credentials/password")]
        [Authorize]
        public async Task<IActionResult> SendPasswordChangeToken()
        {
            string id = User.FindFirstValue("id")??"";

            var result = await _credentialService.SendChangePasswordToken(id);

            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }

        [HttpPut]
        [Route(template: "user/credentials/email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(EmailChangeModel model)
        {
            string id = User.FindFirstValue("id") ?? "";

            var result = await _credentialService.ChangeEmail(model, id);

            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Unauthorized)
                    return NotFound(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }

        [HttpPut]
        [Route(template: "user/credentials/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(CredentialChangeTokenModel model)
        {
            string id = User.FindFirstValue("id") ?? "";

            var result = await _credentialService.ChangePassword(model, id);

            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }
        /*
        public IActionResult Login()
        {
            return Ok();
        }
        public IActionResult Logout()
        {
            return Ok();
        }
        public IActionResult ChangeEmail()
        {
            return Ok();
        }
        public IActionResult ChangePassword() 
        { 
            return Ok();
        }
        */
    }
}
