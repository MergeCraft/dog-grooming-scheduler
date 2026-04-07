using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // 1. Definición de IDs (Esto debe estar)
        var user1Id = "G-01";
        var user2Id = "G-02";
        var user3Id = "G-03";

        var groomer1Id = Guid.Parse("4fb87e5b-89e4-488d-8f20-49299c1c3d53");
        var groomer2Id = Guid.Parse("4d1d2047-8020-473b-a293-35f6d43b0314");
        var groomer3Id = Guid.Parse("2864b026-1c02-4e02-8a77-944aa4fa7e87");

        var fechaFija = new DateTime(2026, 4, 7, 0, 0, 0, DateTimeKind.Utc);

        // 2. SEED DE USUARIOS (A esto me refería, deben estar presentes)
        modelBuilder.Entity<User>().HasData(
            new User { Id = user1Id, UserName = "ana@canina.com", Name = "Ana Martínez", Email = "ana@canina.com", NormalizedUserName = "ANA@CANINA.COM", NormalizedEmail = "ANA@CANINA.COM", EmailConfirmed = true, CreatedAt = fechaFija, SecurityStamp = "STATIC_01", ConcurrencyStamp = "CONC_01" },
            new User { Id = user2Id, UserName = "carlos@canina.com", Name = "Carlos Pérez", Email = "carlos@canina.com", NormalizedUserName = "CARLOS@CANINA.COM", NormalizedEmail = "CARLOS@CANINA.COM", EmailConfirmed = true, CreatedAt = fechaFija, SecurityStamp = "STATIC_02", ConcurrencyStamp = "CONC_02" },
            new User { Id = user3Id, UserName = "lucia@canina.com", Name = "Lucía Gómez", Email = "lucia@canina.com", NormalizedUserName = "LUCIA@CANINA.COM", NormalizedEmail = "LUCIA@CANINA.COM", EmailConfirmed = true, CreatedAt = fechaFija, SecurityStamp = "STATIC_03", ConcurrencyStamp = "CONC_03" }
        );

        // 3. SEED DE PET GROOMERS (También deben estar)
        modelBuilder.Entity<PetGroomer>().HasData(
            new PetGroomer { Id = groomer1Id, UserId = user1Id },
            new PetGroomer { Id = groomer2Id, UserId = user2Id },
            new PetGroomer { Id = groomer3Id, UserId = user3Id }
        );

        var schedules = new List<Schedule>();
        var idsGroomers = new[] { groomer1Id, groomer2Id, groomer3Id };
        int idCounter = 1;

        for (int i = 0; i <= 8; i++)
        {
            var fechaBase = new DateTime(2026, 4, 7).AddDays(i);
            var fechaString = fechaBase.ToString("yyyy-MM-dd");

            foreach (var gId in idsGroomers)
            {
                schedules.Add(new Schedule
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-{idCounter:D12}"),
                    PetGroomerId = gId,
                    Date = DateTime.SpecifyKind(fechaBase, DateTimeKind.Utc),
                    StartTime = DateTime.Parse($"{fechaString}T06:00:00Z").ToUniversalTime(),
                    EndTime = DateTime.Parse($"{fechaString}T14:00:00Z").ToUniversalTime()
                });
                idCounter++;
            }
        }
        modelBuilder.Entity<Schedule>().HasData(schedules);
    }
}