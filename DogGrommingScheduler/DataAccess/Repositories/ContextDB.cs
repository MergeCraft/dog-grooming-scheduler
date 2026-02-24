using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class ContextDB : DbContext
	{
		// El constructor recibe opciones desde Program.cs (la cadena de conexión)
		public ContextDB(DbContextOptions<ContextDB> options) : base(options) { }

		// Esta propiedad representa la tabla "Users" en tu base de datos
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Definimos que el Email debe ser único en la tabla
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
		}
	}
}