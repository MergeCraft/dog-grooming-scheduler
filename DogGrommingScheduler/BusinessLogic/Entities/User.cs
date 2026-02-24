using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; } = string.Empty;

		// NUNCA guardamos la contraseña en texto plano
		// BCrypt convierte "mi123pass" → "$2a$11$xyz..." (hash irreversible)
		public string PasswordHash { get; set; } = string.Empty;

		// Rol para autorización: "Admin", "Client", "Groomer"
		public string Role { get; set; } = "Client";

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
