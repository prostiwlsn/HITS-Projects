using Application.Interfaces;
using Application.Services;
using Common.Extensions;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly IBus _bus;
        public ApplicationController(IProgramService programService, IBus bus)
        {
            _programService = programService;
            _bus = bus;
        }

        [HttpGet]
        [Route("/programs")]
        public async Task<IActionResult> GetPrograms()
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? Guid.Empty.ToString());

            var result = await _programService.GetPrograms(userId);

            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());

            }

            return Ok(result.Load);
        }

        [HttpPost]
        [Route("/program")]
        public async Task<IActionResult> ChooseProgram(Guid programId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? Guid.Empty.ToString());
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _programService.AddProgram(userId, programId);
            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Fail)
                    return BadRequest(result.CastToBase());
                else 
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }
        [HttpPut]
        [Route("/program")]
        public async Task<IActionResult> EditProgram(Guid programId, uint priority)
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? Guid.Empty.ToString());
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _programService.EditProgram(userId, programId, priority);
            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Fail)
                    return BadRequest(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }
        [HttpDelete]
        [Route("/program")]
        public async Task<IActionResult> DeleteProgram(Guid programId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? Guid.Empty.ToString());
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _programService.RemoveProgram(userId, programId);
            if (!result.Success)
            {
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Fail)
                    return BadRequest(result.CastToBase());
                else
                    return BadRequest(result.CastToBase());
            }

            return Ok(result.CastToBase());
        }
    }
}
