using Microsoft.EntityFrameworkCore; 
using Resend;
using AplicationLogic.Interfaces;
using AplicationLogic.Services.Email;
using Hangfire;
using Hangfire.SqlServer;
using AplicationLogic.Services.Scheduler;
using BusinessLogic.RepositoriesInterfaces;
using DataAccess.Repositories;
using BusinessLogic.Interfaces;
using AplicationLogic.ServicesInterfaces;
using AplicationLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. INFRASTRUCTURE ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ContextDB>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.MigrationsAssembly("DataAccess")));

//  Resend (Email)
builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();

// --- 2. DEPENDENCY INJECTION (DI) ---

builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();

builder.Services.AddScoped<IReserveRepository, ReserveRepositoryEF>();
builder.Services.AddScoped<IReserveService, ReserveService>();

// --- 3. HANGFIRE CONFIGURATION ---

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

builder.Services.AddHangfireServer();

// --- 4. WEB SERVICES (API and SWAGGER) ---

// ── REPOSITORIES ───────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ── USE CASES ───────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();

// ── JWT ────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
		};
	});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddOpenApi();
var app = builder.Build();

// --- 5. HTTP MIDDLEWARE PIPELINE ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.Run();
