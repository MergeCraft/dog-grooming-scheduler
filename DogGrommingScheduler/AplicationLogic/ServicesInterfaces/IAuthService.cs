using BusinessLogic.Results;
using Shared.DTOs;
using Shared.DTOs.PetGroomerDtos;

namespace AplicationLogic.ServicesInterfaces
{
	public interface IAuthService
	{
		Task<Result> RegisterAsync(RegisterRequestDto request);
		Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    }
}