using AutoEmail.WebAPI.Options;
using AutoEmail.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHostedService<EmailBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/sendtestmail", async (IEmailService emailService) =>
{
    string mailContent = $"Mail From {Environment.MachineName} at {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}";
    await emailService.SendEmailAsync("to", "Mail From Controller", mailContent);
    return Results.Ok();
})
.WithName("sendtestmail")
.WithOpenApi();


app.Run();

