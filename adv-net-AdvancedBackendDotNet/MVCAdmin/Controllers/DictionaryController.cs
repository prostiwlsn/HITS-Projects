using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAdmin.Interfaces;

namespace MVCAdmin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class DictionaryController : Controller
    {
        private readonly IDictionaryRequestService _requestService;
        public DictionaryController(IDictionaryRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<IActionResult> Index()
        {
            bool isUpdateStarted = await _requestService.GetUpdateState();
            return View(isUpdateStarted);
        }

        public async Task<IActionResult> BeginDocumentTypeUpdate()
        {
            await _requestService.UpdateDocumentTypes();
            return RedirectToAction("UpdateInProgress", "Dictionary");
        }

        public async Task<IActionResult> BeginEducationLevelsUpdate()
        {
            await _requestService.UpdateEducationLevels();
            return RedirectToAction("UpdateInProgress", "Dictionary");
        }

        public async Task<IActionResult> BeginFacultiesUpdate()
        {
            await _requestService.UpdateFaculties();
            return RedirectToAction("UpdateInProgress", "Dictionary");
        }

        public async Task<IActionResult> BeginProgramsUpdate()
        {
            await _requestService.UpdatePrograms();
            return RedirectToAction("UpdateInProgress", "Dictionary");
        }

        public IActionResult UpdateInProgress()
        {
            return View();
        }
    }
}
