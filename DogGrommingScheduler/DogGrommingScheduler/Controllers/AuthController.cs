using Shared.DTOs;
using AplicationLogic.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		bool created = await _authService.RegisterAsync(request);

		if (!created)
			return Conflict(new { message = "El email ya está registrado" });

		return Ok(new { message = "Usuario creado exitosamente" });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		var response = await _authService.LoginAsync(request);

		if (response == null)
			return Unauthorized(new { message = "Credenciales inválidas" });

		return Ok(response);
	}
}