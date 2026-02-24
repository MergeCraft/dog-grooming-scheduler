using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationLogic.DTOs
{
	public class LoginRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		// Solo recibimos estos 2 campos del cliente
	}
}
