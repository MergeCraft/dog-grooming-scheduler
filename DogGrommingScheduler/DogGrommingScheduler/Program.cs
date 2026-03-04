using Microsoft.AspNetCore.Identity.Data;

using Resend;
using AplicationLogic.Interfaces;
using AplicationLogic.Services.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

//Dependencies Injection
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
