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
	public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
	{
		var result = await _authService.RegisterAsync(request);

		if (result.IsFailure)
			return BadRequest(result.Errors);

		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
	{
		var response = await _authService.LoginAsync(request);

		if (response == null)
			return Unauthorized(new { message = "Credenciales inválidas" });

		return Ok(response);
	}
}