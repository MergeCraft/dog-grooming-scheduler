using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AplicationLogic.DTOs;
using AplicationLogic.Mappers;
using AplicationLogic.ServicesInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AplicationLogic.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepo;
		private readonly IConfiguration _config;

		public AuthService(IUserRepository userRepo, IConfiguration config)
		{
			_userRepo = userRepo;
			_config = config;
		}

		public async Task<bool> RegisterAsync(RegisterRequest request)
		{
			if (await _userRepo.ExistsByEmailAsync(request.Email))
				return false;

			string hash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 11);

			var user = new User
			{
				Email = request.Email,
				PasswordHash = hash,
				Role = request.Role
			};

			await _userRepo.CreateAsync(user);
			return true;
		}

		public async Task<AuthResponse?> LoginAsync(LoginRequest request)
		{
			var user = await _userRepo.GetByEmailAsync(request.Email);
			if (user == null) return null;

			bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
			if (!valid) return null;

			string token = GenerateJwtToken(user);
			var expiresAt = DateTime.UtcNow.AddHours(8);

			// El mapper convierte la entidad a DTO antes de salir del servicio
			// A partir de acá, User nunca vuelve a aparecer hacia arriba
			return AuthMapper.ToResponse(user, token, expiresAt);
		}

		private string GenerateJwtToken(User user)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role),
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