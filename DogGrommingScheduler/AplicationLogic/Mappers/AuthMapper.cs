using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Entities;
using AplicationLogic.DTOs;

namespace AplicationLogic.Mappers
{
	public static class AuthMapper
	{
		// Convierte la entidad del dominio en un DTO de salida
		// El PasswordHash es imposible que se filtre porque AuthResponse no lo tiene
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
