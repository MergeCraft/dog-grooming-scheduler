using AplicationLogic.Mappers;
using AplicationLogic.ServicesInterfaces;
using BusinessLogic.Entities;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AplicationLogic.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _config;
        private readonly IClientRepository _clientRepository;
        public AuthService(UserManager<User> userManager, IConfiguration config,IClientRepository clientRepository)
		{
			_userManager = userManager;
			_config = config;
			_clientRepository = clientRepository;
		}

		public async Task<Result> RegisterAsync(RegisterRequestDto request)
		{
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result.Failure(Error.Unexpected);

            await _userManager.AddToRoleAsync(user, request.Role);

            if (request.Role == "Client")
            {
                var newClient = new Client
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id, 
                };

                var clientResult = await _clientRepository.AddAsync(newClient);

                if (!clientResult.IsSuccess)
                {
                    await _userManager.DeleteAsync(user);
                    return Result.Failure(Error.Unexpected);
                }
            }

            return Result.Success();
        }

		public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
		{

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null) return null;

			if (await _userManager.IsLockedOutAsync(user)) return null;

			var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!passwordValid)
			{
				await _userManager.AccessFailedAsync(user); // register failed attempt for lockout
				return null;
			}

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