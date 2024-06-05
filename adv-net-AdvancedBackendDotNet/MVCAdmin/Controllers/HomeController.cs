using Microsoft.AspNetCore.Mvc;
using Common.Models;
using MVCAdmin.Models;
using System.Diagnostics;
using MVCAdmin.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace MVCAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthRequestService _authRequestService;
        private readonly IPersonellRequestService _personalRequestService;

        public HomeController(ILogger<HomeController> logger, IAuthRequestService authRequestService, IPersonellRequestService personalRequestService)
        {
            _logger = logger;
            _authRequestService = authRequestService;
            _personalRequestService = personalRequestService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine("xdddd");
                Console.WriteLine(User.Claims.FirstOrDefault());
                Console.WriteLine();
                Console.WriteLine(Request.Cookies["refreshToken"]);
            }
            Console.WriteLine("Index");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Common.Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authRequestService.Login(model);
                if (response.Success)
                {
                    //HttpContext.Response.Cookies["accessToken"].Value = response.Load.AccessToken;
                    //Response.Cookies.Append("accessToken", response.Load.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddHours(1) });
                    //Console.WriteLine(Request.Cookies["accessToken"]);

                    Response.Cookies.Append("refreshToken", response.Load.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddMonths(1) });

                    string role = await _personalRequestService.GetMyRole(response.Load.AccessToken);
                    string id = await _personalRequestService.GetMyId(response.Load.AccessToken);

                    var claims = new List<Claim>
                    {
                        new Claim("accessToken", response.Load.AccessToken),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("id", id)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return Json( new { redirectUrl = "/", success = true } );
                }
                else
                    return Json(new { success = false, errorMessage = "Enter correct credentials" });
            }

            Console.WriteLine(model.Email, model.Password);
            ModelState.AddModelError(string.Empty, "Enter correct credentials");
            return Json(new { success = false, errorMessage = "Enter correct credentials" });
        }
        
        public IActionResult Register()
        {
            UserRegistrationModel model = new UserRegistrationModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            Console.WriteLine(JsonSerializer.Serialize(model));
            if (ModelState.IsValid)
            {
                var response = await _authRequestService.Register(model);
                if (response.Success)
                {
                    //HttpContext.Response.Cookies["accessToken"].Value = response.Load.AccessToken;
                    //Response.Cookies.Append("accessToken", response.Load.AccessToken, new CookieOptions { Expires = DateTime.UtcNow.AddHours(1) });
                    //Console.WriteLine(Request.Cookies["accessToken"]);

                    return Json(new { success = true });
                }
            }

            ModelState.AddModelError(string.Empty, "Enter correct info");
            return Json(new { success = false, errorMessage = "Enter correct info" });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("refreshToken");
            return RedirectToAction("", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            string token = User.FindFirst("accessToken").Value;
            await _authRequestService.SendPasswordChangeToken(token);

            return View();

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(CredentialChangeTokenModel model)
        {
            string token = User.FindFirst("accessToken").Value;
            await _authRequestService.ChangePassword(model, token);

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        public async Task<IActionResult> ChangeEmail()
        {
            return View();

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(EmailChangeModel model)
        {
            string token = User.FindFirst("accessToken").Value;
            await _authRequestService.ChangeEmail(model, token);

            return RedirectToAction("Index", "Profile");
        }
    }
}
