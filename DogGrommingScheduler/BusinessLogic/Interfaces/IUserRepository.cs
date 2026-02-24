using BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetByEmailAsync(string email);
		Task<bool> ExistsByEmailAsync(string email);
		Task<User> CreateAsync(User user);
	}
}
