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

            // 1. Configuración de Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Email).IsRequired();
                entity.HasIndex(c => c.Email).IsUnique();
            });

            // 2. Configuración de PetGroomer
            modelBuilder.Entity<PetGroomer>(entity =>
            {
                // Si agregaste el Id a la clase, usa g => g.Id. 
                // Si no, mantenemos la Shadow Property por ahora.
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired();

                // Relación: Un Peluquero tiene muchos Schedules
                entity.HasMany(g => g.Schedules)
                      .WithOne(s => s.Groomer)
                      .HasForeignKey(s => s.PetGroomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Configuración de Schedule (La jornada del peluquero)
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.Id);

                // Relación: Un Schedule tiene muchas Reservas
                entity.HasMany(s => s.Reservations)
                      .WithOne(r => r.Schedule)
                      .HasForeignKey(r => r.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict); // Evitamos borrar cascada circular
            });

            // 4. Configuración de Reserve
            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Client)
                      .WithMany(c => c.Reservations)
                      .HasForeignKey(r => r.ClientId) // Sin comillas, usando la propiedad de la clase
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(r => r.PetSize)
                      .HasConversion<string>();
            });
        }
    }
}
