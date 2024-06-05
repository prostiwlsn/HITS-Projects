using Common.Models;
using Common.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAdmin.Interfaces;
using MVCAdmin.Services;
using Common.Result;
using System.Net.Mime;

namespace MVCAdmin.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class ProfileController : Controller
    {
        private IProfileRequestService _profileRequestService;
        public ProfileController(IProfileRequestService profileRequestService)
        {
            _profileRequestService = profileRequestService;
        }

        public async Task<IActionResult> Index()
        {

            string token = User.FindFirst("accessToken").Value;

            var request = await _profileRequestService.GetProfile(token);
            var profile = request.Load;
            if (profile != null)
            {
                ProfileEditModel model = new ProfileEditModel
                {
                    Name = profile.Name,
                    Surname = profile.Surname,
                    SecondName = profile.SecondName,
                    Citizenship = profile.Citizenship,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumber,
                };

                return View(model);
            }
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileEditModel model)
        {
            string token = User.FindFirst("accessToken").Value;
            var result = await _profileRequestService.EditProfile(token, model);
            return Json(result);
        }

        public async Task<IActionResult> Passport(Guid userId)
        {
            var result = await _profileRequestService.GetPassportInfo(new GetPassportInfoRequest { UserId = userId });
            if (!result.Success || result.Load == null) 
                return BadRequest(result);

            EditPassportMessage model = new EditPassportMessage();

            model.UserId = userId;
            model.PassportEditModel.BirthPlace = result.Load.BirthPlace;
            model.PassportEditModel.SeriesNumber = result.Load.SeriesNumber;
            model.PassportEditModel.GivenPlace = result.Load.GivenPlace;
            model.PassportEditModel.GivenDate = result.Load.GivenDate.ToDateTime(TimeOnly.MinValue);

            ViewBag.UserId = userId;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Passport(EditPassportMessage model)
        {
            await _profileRequestService.EditPassportInfo(model);
            return Json(new { Success = true });
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadPassportFile(UploadFileMessage model)
        {
            await _profileRequestService.UploadFile(model);
            return Json(new { Success = true});
        }

        public async Task<IActionResult> EducationDocument(Guid userId)
        {
            var result = await _profileRequestService.GetEducationDocumentInfo(new GetEducationDocumentInfoRequest { UserId = userId });
            if (!result.Success || result.Load == null)
                return BadRequest(result);

            EditEducationalDocumentMessage model = new EditEducationalDocumentMessage();

            model.UserId = userId;
            model.EducationDocumentEditModel = new EducationDocumentEditModel();
            model.EducationDocumentEditModel.Id = result.Load.DocumentTypeId;

            ViewBag.UserId = userId;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EducationDocument(EditEducationalDocumentMessage model)
        {
            await _profileRequestService.EditEducationDocumentInfo(model);
            return Json(new { Success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UploadEducationDocumentFile(UploadFileMessage model)
        {
            await _profileRequestService.UploadFile(model);
            return Json(new { Success = true });
        }

        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            await _profileRequestService.DeleteFile(new DeleteFileMessage { FileId = fileId});
            return Json( new { Success = true });    
        }
        
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var result = await _profileRequestService.GetFile(new GetFileRequest { FileId = fileId });
            return File(result.Load.Bytes, result.Load.ContentType, "doc.pdf");
        }
    }
}
