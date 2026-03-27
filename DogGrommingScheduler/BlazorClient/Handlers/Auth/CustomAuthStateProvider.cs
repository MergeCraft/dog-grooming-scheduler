using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorClient.Handlers.Auth
{
	// Tells Blazor who the current user is by reading the JWT from localStorage.
	// Blazor calls GetAuthenticationStateAsync() every time it needs to know
	// if the user is authenticated (e.g. for [Authorize] or <AuthorizeView>).
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		private readonly IJSRuntime _js;

		// Key used to store/retrieve the token in localStorage
		private const string TokenKey = "authToken";

		public CustomAuthStateProvider(IJSRuntime js)
		{
			_js = js;
		}

		// Called automatically by Blazor to get the current auth state
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

			// No token → return anonymous user
			if (string.IsNullOrWhiteSpace(token))
				return AnonymousState();

			// Parse the claims from the JWT payload
			var claims = ParseClaimsFromJwt(token);

			// Check if the token is expired
			var expiryClaim = claims.FirstOrDefault(c => c.Type == "exp");
			if (expiryClaim != null)
			{
				var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiryClaim.Value));
				if (expiry < DateTimeOffset.UtcNow)
				{
					// Token expired → clear it and return anonymous
					await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
					return AnonymousState();
				}
			}

			var identity = new ClaimsIdentity(claims, "jwt");
			var user = new ClaimsPrincipal(identity);
			return new AuthenticationState(user);
		}

		// Call this after a successful login to notify Blazor the user changed
		public async Task NotifyLoginAsync(string token)
		{
			await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

			var claims = ParseClaimsFromJwt(token);
			var identity = new ClaimsIdentity(claims, "jwt");
			var user = new ClaimsPrincipal(identity);

			// This triggers a re-render on all components that use AuthorizeView
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		// Call this on logout to clear the token and return to anonymous state
		public async Task NotifyLogoutAsync()
		{
			await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
			NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState()));
		}

		// Returns a token from localStorage if it exists, used to attach to HTTP requests
		public async Task<string?> GetTokenAsync()
			=> await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

		// ── Helpers ────────────────────────────────────────────────────────────

		private static AuthenticationState AnonymousState()
			=> new(new ClaimsPrincipal(new ClaimsIdentity()));

		// Decodes the JWT payload (middle part) and converts it to Claims.
		// We do this manually because Blazor WASM can't use JwtSecurityTokenHandler.
		private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			var payload = jwt.Split('.')[1];

			// JWT uses base64url encoding, fix padding before decoding
			payload = payload.Replace('-', '+').Replace('_', '/');
			switch (payload.Length % 4)
			{
				case 2: payload += "=="; break;
				case 3: payload += "="; break;
			}

			var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
			var claims = new List<Claim>();

			using var doc = JsonDocument.Parse(json);
			foreach (var element in doc.RootElement.EnumerateObject())
			{
				// Map standard JWT claim names to ClaimTypes
				var claimType = element.Name switch
				{
					"sub" => ClaimTypes.NameIdentifier,
					"email" => ClaimTypes.Email,
					"role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
						=> ClaimTypes.Role,
					_ => element.Name
				};

				if (element.Value.ValueKind == JsonValueKind.Array)
				{
					// A claim can have multiple values (e.g. multiple roles)
					foreach (var item in element.Value.EnumerateArray())
						claims.Add(new Claim(claimType, item.ToString()));
				}
				else
				{
					claims.Add(new Claim(claimType, element.Value.ToString()));
				}
			}

			return claims;
		}
	}
}