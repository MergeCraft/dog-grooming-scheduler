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

    /// <summary>
    /// Register a new user and return an access token.
    /// </summary>
    /// <remarks>
    /// Creates a new account with the provided registration data. If registration succeeds,
    /// the endpoint automatically logs in the user and returns a JWT access token.
    /// </remarks>
    /// <param name="request">Registration data including email and password.</param>
    /// <returns>On success returns an AuthResponseDto with the access token and expiration. On failure returns validation errors.</returns>
    /// <response code="200">Registration and login successful. Returns AuthResponseDto.</response>
    /// <response code="400">Registration failed. Returns error details.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dtoForLogin = new LoginRequestDto
        {
            Email = request.Email,
            Password = request.Password
        };

        return await Login(dtoForLogin);

    }

    /// <summary>
    /// Authenticate a user and issue a JWT access token.
    /// </summary>
    /// <remarks>
    /// Validates the provided email and password. If the credentials are valid, returns
    /// a JWT token to be used in the 'Authorization' header for protected endpoints.
    /// Example request:
    /// POST /api/Auth/login
    /// { "email": "user@example.com", "password": "Password123!" }
    /// </remarks>
    /// <param name="request">Login data containing email and password.</param>
    /// <returns>AuthResponseDto with the access token and expiration on success.</returns>
    /// <response code="200">Authentication successful. Returns the JWT token.</response>
    /// <response code="401">Authentication failed. Email or password is incorrect.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(response);
    }
}