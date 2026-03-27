using BlazorClient;
using BlazorClient.Handlers.Auth;
using BlazorClient.Services;
using BlazorClient.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Read API base URL from wwwroot/appsettings.json under key "Api:BaseUrl"
var apiBase = builder.Configuration["Api:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBase))
	apiBase = builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

// Register the custom auth state provider.
// AddScoped means one instance per browser session.
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<PetGroomerService>();
builder.Services.AddScoped<ReserveService>();

// Tell Blazor to use our custom provider when it needs to know the auth state.
// This is what makes [Authorize] and <AuthorizeView> work.
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
	sp.GetRequiredService<CustomAuthStateProvider>());

// Register the Blazor-side auth service
builder.Services.AddScoped<AuthService>();

// Required for <AuthorizeView>, <CascadingAuthenticationState>, etc.
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();