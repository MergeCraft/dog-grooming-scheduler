using System;
namespace Shared.DTOs
{
	public class RegisterRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = "Client";
		public string Name { get; set; } = string.Empty;
	}
}