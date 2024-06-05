using Microsoft.AspNetCore.Mvc;
using MVCAdmin.Interfaces;
using Common.Messages;
using Microsoft.AspNetCore.Authorization;
using MVCAdmin.Models;
using Common.Models;
using Newtonsoft.Json.Linq;

namespace MVCAdmin.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class PersonellController : Controller
    {
        private readonly IPersonellRequestService _personellRequestService;
        private readonly IProfileRequestService _profileRequestService;
        public PersonellController(IPersonellRequestService personellRequestService, IProfileRequestService profileRequestService) 
        {
            _personellRequestService = personellRequestService;
            _profileRequestService = profileRequestService;
        }
        public async Task<IActionResult> Index(uint size = 10, uint page = 1, string email = "")
        {
            var result = await _personellRequestService.GetPersonell(new GetPersonellRequest { size = size, page = page, email = email});

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result.Load));
            return View(result.Load);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AssignRoleModel model)
        {
            Console.WriteLine("Adding");
            await _personellRequestService.AssignRole(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddNew()
        {
            return View();
        }
        //[Route("appoint/{userId}")]
        public IActionResult AppointToFaculty(Guid userId) 
        {
            //ViewBag.UserId = userId;
            AppointToFacultyMessage model = new AppointToFacultyMessage { ManagerId = userId};
            return View(model);
        }

        [HttpPost]
        public IActionResult Appoint(AppointToFacultyMessage model)
        {
            _personellRequestService.AppointToFaculty(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid userId) 
        {
            EditPersonellModel model = new EditPersonellModel();
            model.ProfileEditMessage.UserId = userId;
            model.AppointToFacultyMessage.ManagerId = userId;

            var requestProfile = await _profileRequestService.GetApplicantProfile(new GetProfileRequest { Id = userId});
            var profile = requestProfile.Load;

            var personellInfoRequest = await _personellRequestService.GetManagerInfo(new GetManagerInfoRequest { ManagerId = userId });
            var info = personellInfoRequest.Load;

            if (profile != null && info != null)
            {
                model.AppointToFacultyMessage.ManagerId = userId;

                model.ProfileEditMessage.ProfileEditModel.Name = profile.Name;
                model.ProfileEditMessage.ProfileEditModel.Surname = profile.Surname;
                model.ProfileEditMessage.ProfileEditModel.SecondName = profile.SecondName;
                model.ProfileEditMessage.ProfileEditModel.Citizenship = profile.Citizenship;
                model.ProfileEditMessage.ProfileEditModel.Gender = profile.Gender;
                model.ProfileEditMessage.ProfileEditModel.PhoneNumber = profile.PhoneNumber;

                model.AssignRoleMessage.Email = profile.Email;
                model.AssignRoleMessage.Role = info.Role;

                if (info.Role == Roles.Manager && info.FacultyId != null)
                {
                    model.AppointToFacultyMessage.FacultyId = info.FacultyId.ToString();
                }

                return View(model);
            }

            
            return RedirectToAction("", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPersonellModel model)
        {
            await _profileRequestService.EditApplicantProfile(model.ProfileEditMessage);
            await _personellRequestService.AssignRole(model.AssignRoleMessage);
            if (model.AssignRoleMessage.Role == Roles.Manager)
            {
                await _personellRequestService.AppointToFaculty(model.AppointToFacultyMessage);
            }
            return Json(new { Success = true });
        }

        public async Task<IActionResult> Delete(string email)
        {
            await _personellRequestService.AssignRole(new AssignRoleModel { Email = email, Role = Roles.None});
            return RedirectToAction("Index");
        }
    }
}
