using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared.DTOs;
using AplicationLogic.Mappers;
using AplicationLogic.ServicesInterfaces;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AplicationLogic.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _config;

		public AuthService(UserManager<User> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		// Returns null on success, or a list of error messages on failure
		public async Task<IEnumerable<string>?> RegisterAsync(RegisterRequest request)
		{
			var user = new User
			{
				UserName = request.Email,
				Email = request.Email,
				Name = request.Name
			};

			// Identity hashes the password internally
			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
				return result.Errors.Select(e => e.Description); // return Identity's actual errors

			// Assign role
			await _userManager.AddToRoleAsync(user, request.Role);
			return null; // null means success
		}

		public async Task<AuthResponse?> LoginAsync(LoginRequest request)
		{

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null) return null;

			// Check if account is locked out
			if (await _userManager.IsLockedOutAsync(user)) return null;

			// Verify password without cookies (JWT only)
			var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!passwordValid)
			{
				await _userManager.AccessFailedAsync(user); // register failed attempt for lockout
				return null;
			}

			// Reset failed attempts counter on successful login
			await _userManager.ResetAccessFailedCountAsync(user);

			var roles = await _userManager.GetRolesAsync(user);
			string role = roles.FirstOrDefault() ?? "Client";
			string token = GenerateJwtToken(user, role);
			var expiresAt = DateTime.UtcNow.AddHours(8);

			return AuthMapper.ToResponse(user, token, role, expiresAt);
		}

		private string GenerateJwtToken(User user, string role)
		{
			var claims = new[]
			{
                // Id is string (GUID) from IdentityUser
                new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email!),
				new Claim(ClaimTypes.Role, role),
			};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(8),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}