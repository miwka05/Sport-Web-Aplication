using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using kursovOsn.Server.Models;
using System;
using System.Linq;

namespace kursovOsn.Server.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (!context.Tournament_Formats.Any())
            {
                var format = new Tournament_Format { Name = "Round Robin" };
                context.Tournament_Formats.Add(format);
                await context.SaveChangesAsync();

                // и потом используем format.ID
            }

            if (!context.Sports.Any())
            {
                var football = new Sport { Name = "Football" };
                var basketball = new Sport { Name = "Basketball" };
                var tennis = new Sport { Name = "Tennis" };

                context.Sports.AddRange(football, basketball,tennis);
                await context.SaveChangesAsync(); // обязательно сохраняем, чтобы получить ID

                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                        new Team { Name = "Tigers", Sport_ID = football.ID, City = "NY", Age = 10 },
                        new Team { Name = "Lions", Sport_ID = football.ID, City = "LA", Age = 12 }
                    );
                }

                if (!context.Tournaments.Any())
                {
                    context.Tournaments.Add(new Tournament
                    {
                        Name = "Championship 2025",
                        Sport_ID = tennis.ID,
                        Format_ID = 1, // убедись, что Format с таким ID тоже существует
                        Start = DateTime.UtcNow,
                        End = DateTime.UtcNow.AddMonths(1),
                        Adress = "Main Stadium",
                        Info = "Top-level event",
                        Age = "18+",
                        Pol = "Mixed",
                        Status = "Upcoming",
                        Creator_ID = "707778db - fbfe - 4cb4 - a4b7 - b205872f9611",
                        solo = true
                    });
                }

                await context.SaveChangesAsync();
            }
        }


    }
}
