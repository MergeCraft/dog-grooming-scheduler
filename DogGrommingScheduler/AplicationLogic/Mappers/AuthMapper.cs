using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Entities;
using Shared.DTOs;

namespace AplicationLogic.Mappers
{
	public static class AuthMapper
	{
		
		public static AuthResponse ToResponse(User user, string token, DateTime expiresAt)
		{
			return new AuthResponse
			{
				Token = token,
				Email = user.Email,
				Role = user.Role,
				ExpiresAt = expiresAt
			};
		}
	}
}
