using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MVCAdmin.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MVCAdmin.Middleware
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public TokenRefreshMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (context.User.Identity != null && !context.User.Identity.IsAuthenticated && refreshToken != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var authRequestService = scope.ServiceProvider.GetRequiredService<IAuthRequestService>();
                    
                    var response = await authRequestService.Refresh(refreshToken);

                    if (response != null && response.Load != null && response.Success) 
                    {
                        context.Response.Cookies.Append("refreshToken", response.Load.RefreshToken, new CookieOptions { Expires = DateTime.UtcNow.AddMonths(1) });

                        var personellRequestService = scope.ServiceProvider.GetRequiredService<IPersonellRequestService>();
                        string role = await personellRequestService.GetMyRole(response.Load.AccessToken);
                        string id = await personellRequestService.GetMyId(response.Load.AccessToken);
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
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    }

                }
            }

            await _next(context);
        }
    }
}
