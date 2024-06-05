using Dictionary.Interfaces;
using Dictionary.Data;
using Microsoft.EntityFrameworkCore;
using Dictionary.Services;
using EasyNetQ;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using Dictionary.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Dictionary.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/*
 * .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
}); ;
*/

builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("BusConnection")));
builder.Services.AddDbContext<DictionaryDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddScoped<IDictionaryUpdateService, DictionaryUpdateService>();
builder.Services.AddScoped<IDictionaryService, DictionaryService>();

builder.Services.AddScoped<DictionaryMessageHandler>();

//builder.Services.AddHostedService<ProgramUpdateService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetService<IBus>().SetupListeners(app);

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DictionaryDbContext>();
    dbContext.Database.Migrate();

    var httpClient = serviceScope.ServiceProvider.GetRequiredService<HttpClient>();
    string username = app.Configuration.GetSection("ExternalService:Username").Value;
    string password = app.Configuration.GetSection("ExternalService:Password").Value;

    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
