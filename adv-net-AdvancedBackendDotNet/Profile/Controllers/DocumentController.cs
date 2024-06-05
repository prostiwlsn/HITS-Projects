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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private IDocumentService _documentService { get; set; }
        private IBus _bus;
        public DocumentController(IDocumentService documentService, IBus bus)
        {
            _documentService = documentService;
            _bus = bus;
        }

        [HttpGet]
        [Route("/profile/document/passport")]
        public async Task<IActionResult> GetPassportInformation()
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));

            var result = await _documentService.GetPassportInfo(userId);

            if (result.Success)
                return Ok(result.Load);
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }
        [HttpGet]
        [Route("/profile/document/educationDocument")]
        public async Task<IActionResult> GetEducationDocumentInformation()
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));

            var result = await _documentService.GetEducationDocumentInfo(userId);

            if (result.Success)
                return Ok(result.Load);
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }

        [HttpPut]
        [Route("/profile/document/passport")]
        public async Task<IActionResult> EditPassportInformation(PassportEditModel model)
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _documentService.EditPassportInfo(userId, model);

            if (result.Success)
                return Ok();
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }
        [HttpPut]
        [Route("/profile/document/educationDocument")]
        public async Task<IActionResult> EditEducationDocumentInformation(EducationDocumentEditModel model)
        {
            //поменять модель
            var userId = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _documentService.EditEducationDocumentInfo(userId, model);

            if (result.Success)
                return Ok();
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("/profile/document/passport/scan")]
        public async Task<IActionResult> GetPassportFile(Guid fileId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));
            var result = await _documentService.GetFile(fileId, userId);

            if (!result.Success)
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Unauthorized)
                    return Unauthorized(result.CastToBase());

            string contentType = "application/pdf";

            return PhysicalFile(result.Load, contentType, "passport.pdf");
        }
        [HttpGet]
        [Route("/profile/document/educationDocument/scan")]
        public async Task<IActionResult> GetEducationDocumentFile(Guid fileId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));
            var result = await _documentService.GetFile(fileId, userId);

            if (!result.Success)
                if (result.ResultMessage == ResultMessage.NotFound)
                    return NotFound(result.CastToBase());
                else if (result.ResultMessage == ResultMessage.Unauthorized)
                    return Unauthorized(result.CastToBase());

            string contentType = "application/pdf";

            return PhysicalFile(result.Load, contentType, "educationDocument.pdf");
        }

        [HttpPost]
        [Route("/profile/document/passport/scan")]
        public async Task<IActionResult> UploadPassportFile(FileModel model)
        {
            Guid id = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(id);
            if (isClosed)
                return Forbid();

            var result = await _documentService.UploadPassportFile(id, model);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        [Route("/profile/document/educationDocument/scan")]
        public async Task<IActionResult> UploadEducationDocumentFile(FileModel model)
        {
            Guid id = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(id);
            if (isClosed)
                return Forbid();

            var result = await _documentService.UploadEducationDocumentFile(id, model);
            if (!result.Success)
                return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("/profile/document/passport/scan")]
        public async Task<IActionResult> DeletePassportFile(Guid fileId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _documentService.DeleteFile(fileId, userId);

            if (result.Success)
                return Ok();
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else if (result.ResultMessage == ResultMessage.Unauthorized)
                return Unauthorized(result.CastToBase());
            else
                return BadRequest();
        }
        [HttpDelete]
        [Route("/profile/document/educationDocument/scan")]
        public async Task<IActionResult> DeleteEducationDocumentFile(Guid fileId)
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));
            bool isClosed = _bus.GetApplicationStatus(userId);
            if (isClosed)
                return Forbid();

            var result = await _documentService.DeleteFile(fileId, userId);

            if (result.Success)
                return Ok();
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else if (result.ResultMessage == ResultMessage.Unauthorized)
                return Unauthorized(result.CastToBase());
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("/profile/document/passport/scans")]
        public async Task<IActionResult> GetPassportFiles()
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));

            var result = await _documentService.GetPassportFiles(userId);

            if (result.Success)
                return Ok(result.Load);
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("/profile/document/educationDocument/scans")]
        public async Task<IActionResult> GetEducationDocumentFiles()
        {
            var userId = Guid.Parse(User.FindFirstValue("id"));

            var result = await _documentService.GetEducationDocumentFiles(userId);

            if (result.Success)
                return Ok(result.Load);
            else if (result.ResultMessage == ResultMessage.NotFound)
                return NotFound(result.CastToBase());
            else
                return BadRequest();
        }
    }
}
