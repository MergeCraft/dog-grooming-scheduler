using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<PetGroomer> PetGroomers { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Client configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Email).IsRequired();
                entity.HasIndex(c => c.Email).IsUnique();
            });

            // 2. PetGroomer configuration
            modelBuilder.Entity<PetGroomer>(entity =>
            {
                // If you added the Id to the class, use g => g.Id.
                // If not, keep the shadow property for now.
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired();

                // Relationship: a groomer has many schedules
                entity.HasMany(g => g.Schedules)
                      .WithOne(s => s.Groomer)
                      .HasForeignKey(s => s.PetGroomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Schedule configuration (the groomer's working day)
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.Id);

                // Relationship: a schedule has many reserves
                entity.HasMany(s => s.Reservations)
                      .WithOne(r => r.Schedule)
                      .HasForeignKey(r => r.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict); // Avoid circular cascade deletes
            });

            // 4. Reserve configuration
            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Client)
                      .WithMany(c => c.Reservations)
                      .HasForeignKey(r => r.ClientId) // Without quotes, using the class property
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(r => r.PetSize)
                      .HasConversion<string>();
            });
        }
    }
}
