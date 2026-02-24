using AplicationLogic.DTOs;

namespace AplicationLogic.ServicesInterfaces
{
	public interface IAuthService
	{
		Task<bool> RegisterAsync(RegisterRequest request);
		Task<AuthResponse?> LoginAsync(LoginRequest request);
	}
}