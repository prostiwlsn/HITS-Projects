using EasyNetQ;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVCAdmin.Interfaces;
using MVCAdmin.Services;
using Common.AuthPolicy;
using MVCAdmin.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("BusConnection")));

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddScoped<IAuthRequestService, AuthRequestService>();
builder.Services.AddScoped<IPersonellRequestService, PersonellRequestService>();
builder.Services.AddScoped<IProfileRequestService, ProfileRequestService>();
builder.Services.AddScoped<IDictionaryRequestService, DictionaryRequestService>();
builder.Services.AddScoped<IApplicationRequestService, ApplicationRequestService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = false;
    });

builder.AddAdminOnlyPolicy();
builder.AddMainManagerOnlyPolicy();
builder.AddManagerOnlyPolicy();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var _personellRequestService = serviceScope.ServiceProvider.GetService<IPersonellRequestService>();
    await _personellRequestService.CreateAdminAccount();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<TokenRefreshMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
