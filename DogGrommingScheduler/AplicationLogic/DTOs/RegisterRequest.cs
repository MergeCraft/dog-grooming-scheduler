using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationLogic.DTOs
{
	public class RegisterRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = "Client";
	}
}
