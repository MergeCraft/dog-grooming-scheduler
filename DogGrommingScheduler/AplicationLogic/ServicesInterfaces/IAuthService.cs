using Shared.DTOs;

namespace AplicationLogic.ServicesInterfaces
{
	public interface IAuthService
	{
		Task<IEnumerable<string>?> RegisterAsync(RegisterRequest request);
		Task<AuthResponse?> LoginAsync(LoginRequest request);
	}
}