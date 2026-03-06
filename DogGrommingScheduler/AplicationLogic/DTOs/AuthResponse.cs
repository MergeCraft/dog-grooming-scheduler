using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationLogic.DTOs
{
	public class AuthResponse
	{
		public string Token { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public DateTime ExpiresAt { get; set; }
		// Devolvemos el token y datos básicos, NUNCA el PasswordHash
	}
}
