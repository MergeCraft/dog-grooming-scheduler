using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Entities
{
	public class User : IdentityUser
	{
		// IdentityUser ya tiene: Id, Email, PasswordHash, UserName, etc.
		// Solo agregás lo que es tuyo
		public string Name { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
