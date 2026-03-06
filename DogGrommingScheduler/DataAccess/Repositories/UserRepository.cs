using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ContextDB _db;

		public UserRepository(ContextDB db)
		{
			_db = db; // EF Core vive AQUÍ, no se filtra hacia arriba
		}

		public async Task<User?> GetByEmailAsync(string email)
			=> await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

		public async Task<bool> ExistsByEmailAsync(string email)
			=> await _db.Users.AnyAsync(u => u.Email == email);

		public async Task<User> CreateAsync(User user)
		{
			_db.Users.Add(user);
			await _db.SaveChangesAsync();
			return user;
		}
	}
}