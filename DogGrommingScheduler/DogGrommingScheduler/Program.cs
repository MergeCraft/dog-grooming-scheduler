using Microsoft.AspNetCore.Identity.Data;

using Resend;
using AplicationLogic.Interfaces;
using AplicationLogic.Services.Email;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();

//Dependencies Injection
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();

//Hangfire Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true
    }));

// server that will process the jobs in the background
builder.Services.AddHangfireServer();

// Add services to the container.

builder.Services.AddControllers();

// El generador de Swagger
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Dashboard Hangfire
app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.Run();
