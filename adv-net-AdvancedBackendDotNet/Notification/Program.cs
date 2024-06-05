using EasyNetQ;
using Notification;
using Notification.Services;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SmtpClient>(
    new SmtpClient(builder.Configuration.GetSection("Smtp:Host").Value,
    builder.Configuration.GetSection("Smtp:Port").Get<int>()));

builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("BusConnection")));

builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    //var client = scope.ServiceProvider.GetRequiredService<EmailService>();

    //await client.SendEmail("admin@admin.com", "test");
}

app.Services.GetService<IBus>().SetupListener(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
