using Common.Messages;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAdmin.Interfaces;
using RabbitMQ.Client;
using System.Security.Claims;

namespace MVCAdmin.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class ApplicationController : Controller
    {
        private readonly IApplicationRequestService _applicationRequestService;
        private readonly IProfileRequestService _profileRequestService;
        public ApplicationController(IApplicationRequestService applicationRequestService, IProfileRequestService profileRequestService) 
        { 
            _applicationRequestService = applicationRequestService;
            _profileRequestService = profileRequestService;
        }

        public async Task<IActionResult> Index(
            [FromQuery] string? name = null, [FromQuery] List<string>? faculties = null,
            [FromQuery] ApplicationStatus status = ApplicationStatus.Any, 
            [FromQuery] string? managerId = null, [FromQuery] bool isDescending = false,
            [FromQuery] uint size = 10, [FromQuery] uint page = 1)
        {
            GetApplicationsRequest request = new GetApplicationsRequest 
            {
                size = size,
                page = page,
                name = name??string.Empty,
                faculties = faculties != null ? faculties.ToList() : new List<string>(),
                status = status,
                isManagerAppointed = managerId == null ? null : (managerId == "none" ? false : true),
                managerId = managerId == null || managerId == "none" ? Guid.Empty : Guid.Parse(managerId),
                isDescending = isDescending,
            };
            var response = await _applicationRequestService.GetApplications(request);

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
            return View(response.Load);
        }

        public async Task<IActionResult> Edit(Guid userId)
        {
            var application = await _applicationRequestService.GetApplication(new GetApplicationRequest { UserId = userId });
            var profile = await _profileRequestService.GetApplicantProfile(new GetProfileRequest { Id = userId });

            if (!application.Success || !profile.Success)
                return NotFound();

            EditApplicationModel model = new EditApplicationModel();
            model.EditApplicationStatusMessage.UserId = userId;
            model.ProfileEditMessage.UserId = userId;
            model.AppointManagerMessage.UserId = userId;

            model.ProfileEditMessage.ProfileEditModel = new ProfileEditModel
            {
                Name = profile.Load.Name,
                Surname = profile.Load.Surname,
                SecondName = profile.Load.SecondName,
                Citizenship = profile.Load.Citizenship,
                Gender = profile.Load.Gender,
                PhoneNumber = profile.Load.PhoneNumber,
            };

            model.AppointManagerMessage.ManagerId = application.Load.ManagerId.ToString()??Guid.Empty.ToString();
            model.EditApplicationStatusMessage.Status = application.Load.Status;    
            //var profile = 

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditApplicationModel model)
        {
            Console.WriteLine("xdd");

            if (User.Claims.First(claim => claim.Type == ClaimTypes.Role).Value == "Manager" && model.AppointManagerMessage.ManagerId != User.Claims.First(claim => claim.Type == "id").Value)
                return Unauthorized();
            Console.WriteLine("xddd");

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));

            await _applicationRequestService.EditApplication(model);

            Console.WriteLine("xddddd");
            return Json(new { Success = true });
        }

        public async Task<IActionResult> Programs(Guid userId)
        {
            var response = await _applicationRequestService.GetPrograms(new GetProgramsMessage { UserId = userId});
            if (response == null || response.Load == null || !response.Success)
                return RedirectToAction("Index");

            EditProgramsMessage model =  new EditProgramsMessage { UserId = userId, Programs = response.Load.OrderBy(p => p.Priority)
                .Select(p => new ProgramPriorityModel { Priority = p.Priority, Program = p.ProgramId}).ToList() };

            ViewBag.ProgramNames = response.Load.OrderBy(p => p.Priority).Select(p => p.ProgramName).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditPrograms(EditProgramsMessage model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));
            }
            await _applicationRequestService.EditPrograms(model);
            return Json(new { Success = true });
        }

        public async Task<IActionResult> DeleteProgram(Guid userId, Guid programId)
        {
            await _applicationRequestService.DeleteProgram(new DeleteProgramMessage { UserId = userId, ProgramId = programId });
            return RedirectToAction("Programs", userId);
        }
    }
}
