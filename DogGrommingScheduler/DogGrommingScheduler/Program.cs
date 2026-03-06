using Microsoft.EntityFrameworkCore; 
using Resend;
using AplicationLogic.Interfaces;
using AplicationLogic.Services.Email;
using Hangfire;
using Hangfire.SqlServer;
using AplicationLogic.Services.Scheduler;
using BusinessLogic.RepositoriesInterfaces;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE DATOS E INFRAESTRUCTURA ---

// Obtenemos la conexión primero para que esté disponible abajo
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro del DbContext (Entity Framework)
builder.Services.AddDbContext<ContextDB>(options =>
    options.UseSqlServer(connectionString));

// Configuración de Resend (Email)
builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();

// --- 2. INYECCIÓN DE DEPENDENCIAS ---

// Email
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();

// Repositorios y Servicios de Reserva
builder.Services.AddScoped<IReserveRepository, ReserveRepositoryEF>();
builder.Services.AddScoped<IReserveService, ReserveService>();

// --- 3. CONFIGURACIÓN DE HANGFIRE ---

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

// Servidor de Hangfire para procesar tareas
builder.Services.AddHangfireServer();

// --- 4. SERVICIOS WEB (API Y SWAGGER) ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger en .NET 8/9
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- 5. PIPELINE DE MIDDLEWARE (HTTP) ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Dashboard de Hangfire (para ver los emails programados)
app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.Run();
