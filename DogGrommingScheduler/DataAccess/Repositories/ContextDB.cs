using BusinessLogic.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Repositories
{
	public class ContextDB : IdentityDbContext<User>
	{
		public ContextDB(DbContextOptions<ContextDB> options) : base(options) { }

		public DbSet<Client> Clients { get; set; }
		public DbSet<PetGroomer> PetGroomers { get; set; }
		public DbSet<Reserve> Reserves { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// 1. Client configuration
			modelBuilder.Entity<Client>(entity =>
			{
				entity.HasKey(c => c.Id);
				entity.HasOne(c => c.User)
					  .WithOne()
					  .HasForeignKey<Client>(c => c.UserId);
			});

			// 2. PetGroomer configuration
			modelBuilder.Entity<PetGroomer>(entity =>
			{
				entity.HasKey(g => g.Id);

				// Relationship with Identity User
				entity.HasOne(g => g.User)
					  .WithOne()
					  .HasForeignKey<PetGroomer>(g => g.UserId);

				entity.HasMany(g => g.Schedules)
					  .WithOne()
					  .HasForeignKey(s => s.PetGroomerId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			// 3. Schedule configuration
			modelBuilder.Entity<Schedule>(entity =>
			{
				entity.HasKey(s => s.Id);

				entity.HasMany(s => s.Reservations)
					  .WithOne()
					  .HasForeignKey(r => r.ScheduleId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
            // Configuración para asegurar que las fechas se traten correctamente
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(s => s.Date).HasColumnType("timestamp with time zone");
                entity.Property(s => s.StartTime).HasColumnType("timestamp with time zone");
                entity.Property(s => s.EndTime).HasColumnType("timestamp with time zone");
            });
            // 4. Reserve configuration
            modelBuilder.Entity<Reserve>(entity =>
			{
				entity.HasKey(r => r.Id);

				entity.HasOne(r => r.Client)
					  .WithMany(c => c.Reservations)
					  .HasForeignKey(r => r.ClientId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.Property(r => r.PetSize)
					  .HasConversion<string>();
			});
            DbSeeder.Seed(modelBuilder);
        }
	}
}