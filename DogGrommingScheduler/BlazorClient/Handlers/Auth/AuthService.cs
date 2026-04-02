using BlazorClient.Handlers.Auth;
using Shared.DTOs;
using System.Net.Http.Json;

namespace BlazorClient.Services.Auth
{
	// Handles all authentication HTTP calls to the API.
	// Acts as the only entry point for login/register from the UI layer.
	public class AuthService
	{
		private readonly HttpClient _http;
		private readonly CustomAuthStateProvider _authStateProvider;

		public AuthService(HttpClient http, CustomAuthStateProvider authStateProvider)
		{
			_http = http;
			_authStateProvider = authStateProvider;
		}

		// Sends login credentials to the API and stores the returned JWT on success.
		// Returns null if the credentials are invalid or the request fails.
		public async Task<string?> LoginAsync(LoginRequestDto request)
		{
			try
			{
				var response = await _http.PostAsJsonAsync("api/Auth/login", request);

				if (!response.IsSuccessStatusCode)
					return "Incorrect email or password.";

				var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

				if (result?.Token is null)
					return "Unexpected error, please try again.";

				// Store the token and notify Blazor that the user is now authenticated
				await _authStateProvider.NotifyLoginAsync(result.Token);

				return null; // null means success
			}
			catch
			{
				return "Could not connect to the server.";
			}
		}

        // Sends registration data to the API.
        // On success, automatically logs the user in (stores token and notifies state).
        // Returns null on success, or an error message string on failure.
        public async Task<string?> RegisterAsync(RegisterRequestDto request)
		{
			try
			{
				var response = await _http.PostAsJsonAsync("api/Auth/register", request);

				if (!response.IsSuccessStatusCode)
				{
					// Read the actual error messages from the API
					var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
					if (error?.Messages != null)
						return string.Join(", ", error.Messages);

					return "Registration failed.";
				}           
               
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                if (result?.Token is null)
                    return "Account created, but autologin failed. Please log in manually.";

                await _authStateProvider.NotifyLoginAsync(result.Token);

                return null; // null means success
			}
			catch
			{
				return "Could not connect to the server.";
			}
		}

		// Helper class to deserialize the error response from the API
		private class ErrorResponse
		{
			public IEnumerable<string>? Messages { get; set; }
		}

		// Clears the stored token and notifies Blazor the user logged out.
		public async Task LogoutAsync()
		{
			await _authStateProvider.NotifyLogoutAsync();
		}
	}
}