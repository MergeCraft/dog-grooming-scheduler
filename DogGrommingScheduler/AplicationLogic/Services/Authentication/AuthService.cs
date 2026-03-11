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
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _config;

		public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
		}

		public async Task<bool> RegisterAsync(RegisterRequest request)
		{
			var user = new User
			{
				UserName = request.Email,
				Email = request.Email,
				Name = request.Name
			};

			// Identity hashea el password internamente, ya no necesitamos BCrypt
			var result = await _userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded) return false;

			// Asignar el rol
			await _userManager.AddToRoleAsync(user, request.Role);
			return true;
		}

		public async Task<AuthResponse?> LoginAsync(LoginRequest request)
		{
			// Verifica credenciales y maneja lockout automáticamente
			var result = await _signInManager.PasswordSignInAsync(
				request.Email,
				request.Password,
				isPersistent: false,
				lockoutOnFailure: true
			);

			if (result.IsLockedOut) return null;
			if (!result.Succeeded) return null;

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null) return null;

			// Los roles ya no están en user.Role, se consultan a Identity
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
                // Id ahora es string (GUID) porque viene de IdentityUser
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