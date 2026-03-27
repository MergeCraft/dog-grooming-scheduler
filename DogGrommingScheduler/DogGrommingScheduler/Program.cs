using AplicationLogic.Interfaces;
using AplicationLogic.Services;
using AplicationLogic.Services.Email;
using AplicationLogic.Services.PetGroomer;
using AplicationLogic.Services.Scheduler;
using AplicationLogic.ServicesInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.RepositoryInterfaces;
using DataAccess.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resend;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
var blazorOrigins = builder.Configuration.GetSection("BlazorClient:AllowedOrigins").Get<string[]>();
var enableCorsPolicy = (blazorOrigins != null && blazorOrigins.Length > 0) || builder.Environment.IsDevelopment();

if (enableCorsPolicy)
{
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(name: "BlazorClientPolicy", policy =>
		{
			if (blazorOrigins != null && blazorOrigins.Length > 0)
			{
				policy.WithOrigins(blazorOrigins)
					  .AllowAnyHeader()
					  .AllowAnyMethod()
					  .AllowCredentials();
			}
			else
			{
				policy.AllowAnyOrigin()
					  .AllowAnyHeader()
					  .AllowAnyMethod();
			}
		});
	});
}

// --- 1. INFRASTRUCTURE ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ContextDB>(options =>
	options.UseSqlServer(connectionString, sqlOptions =>
		sqlOptions.MigrationsAssembly("DataAccess")));

// Resend (Email)
builder.Services.Configure<ResendClientOptions>(options =>
{
	options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();

// --- 2. DEPENDENCY INJECTION (DI) ---

builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();
builder.Services.AddScoped<IReserveService, ReserveService>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepositoryEF>();
builder.Services.AddScoped<IPetGroomerRepository, PetGroomerRepositoryEF>();
builder.Services.AddScoped<IClientRepository, ClientRepositoryEF>();
builder.Services.AddScoped<IPetGroomerService, PetGroomerService>();

// ── IDENTITY ───────────────────────────────────────────
builder.Services.AddIdentityCore<User>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = false;
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ContextDB>()
.AddSignInManager()
.AddDefaultTokenProviders();

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
// IUserRepository eliminado, Identity lo reemplaza
builder.Services.AddScoped<IReserveRepository, ReserveRepositoryEF>();

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

var app = builder.Build();

// --- 5. HTTP MIDDLEWARE PIPELINE ---

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var blazorOriginsConfigured = builder.Configuration.GetSection("BlazorClient:AllowedOrigins").Get<string[]>() is { Length: > 0 };
if (blazorOriginsConfigured || app.Environment.IsDevelopment())
{
	app.UseCors("BlazorClientPolicy");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	string[] roles = ["Client", "Groomer", "Admin"];

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole(role));
	}
}

app.Run();