using kursovOsn.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace kursovOsn.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Sport_Statistic> Sport_Statistics { get; set; }
        public DbSet<Tournament_Stages> Tournament_Stages { get; set; }
        public DbSet<Tournament_Format> Tournament_Formats { get; set; }
        public DbSet<Tournament_Entry> Tournament_Entries { get; set; }
        public DbSet<Tournament_participant> Tournament_Participants { get; set; }
        public DbSet<Team_entry> Team_Entries { get; set; }
        public DbSet<Team_player> Team_Players { get; set; }
        public DbSet<Player_Statistic> Player_Statistics { get; set; }
        public DbSet<Team_Statistic> Team_Statistics { get; set; }
        public DbSet<Tournament_Standings> Tournament_Standings { get; set; }
        public DbSet<Play_Off> Play_Offs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Конфигурация отношений
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player1)
                .WithMany(u => u.Match1)
                .HasForeignKey(m => m.Player1_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany(u => u.Match2)
                .HasForeignKey(m => m.Player2_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team1)
                .WithMany(t => t.MatchesAsTeam1)
                .HasForeignKey(m => m.Team1_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team2)
                .WithMany(t => t.MatchesAsTeam2)
                .HasForeignKey(m => m.Team2_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Sport)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.Sport_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.Tournament_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Stage)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.Stage_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Sport)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.Sport_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Play_Off>()
                .HasOne(p => p.NextMatchLoser)
                .WithMany(m => m.Play_Offs_L)
                .HasForeignKey(p => p.Next_Match_Loser_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Play_Off>()
                .HasOne(p => p.NextMatchWinner)
                .WithMany(m => m.Play_Offs_W)
                .HasForeignKey(p => p.Next_Match_Winner_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Play_Off>()
                .HasOne(p => p.Match)
                .WithMany(m => m.Play_Offs)
                .HasForeignKey(p => p.Match_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player_Statistic>()
                .HasOne(p => p.Match)
                .WithMany(m => m.PlStat)
                .HasForeignKey(p => p.Match_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player_Statistic>()
                .HasOne(p => p.Player)
                .WithMany(m => m.Stat)
                .HasForeignKey(p => p.Player_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Sport_Statistic>()
                .HasOne(ss => ss.Sport)
                .WithMany(s => s.Statistics)
                .HasForeignKey(p => p.Sport_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Sport)
                .WithMany(s => s.Teams)
                .HasForeignKey(t => t.Sport_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_entry>()
                .HasOne(t => t.Team)
                .WithMany(s => s.TeamEntries)
                .HasForeignKey(t => t.Team_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_entry>()
                .HasOne(t => t.User)
                .WithMany(s => s.Team_e)
                .HasForeignKey(t => t.User_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_player>()
                .HasOne(t => t.User)
                .WithMany(s => s.Teams)
                .HasForeignKey(t => t.ID_User)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_player>()
                .HasOne(t => t.Team)
                .WithMany(s => s.TeamPlayers)
                .HasForeignKey(t => t.ID_Team)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_Statistic>()
                .HasOne(p => p.Team)
                .WithMany(m => m.Statistics)
                .HasForeignKey(p => p.Team_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_Statistic>()
                .HasOne(p => p.Match)
                .WithMany(m => m.TeamStat)
                .HasForeignKey(p => p.Match_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Sport)
                .WithMany(s => s.Tournaments)
                .HasForeignKey(t => t.Sport_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Format)
                .WithMany(s => s.Tournaments)
                .HasForeignKey(t => t.Format_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Entry>()
                .HasOne(t => t.Team)
                .WithMany(s => s.TournamentEntries)
                .HasForeignKey(t => t.ID_Team)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Entry>()
                .HasOne(t => t.User)
                .WithMany(s => s.Tournaments)
                .HasForeignKey(t => t.ID_User)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Entry>()
                .HasOne(t => t.Tournament)
                .WithMany(s => s.Entries)
                .HasForeignKey(t => t.ID_Tournament)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_participant>()
                .HasOne(t => t.Team)
                .WithMany(s => s.TournamentParticipants)
                .HasForeignKey(t => t.Team_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_participant>()
                .HasOne(t => t.User)
                .WithMany(s => s.Tournament_p)
                .HasForeignKey(t => t.User_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_participant>()
                .HasOne(t => t.Tournament)
                .WithMany(s => s.Participants)
                .HasForeignKey(t => t.Tournament_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Stages>()
                .HasOne(t => t.Tournament)
                .WithMany(s => s.Stages)
                .HasForeignKey(t => t.Tournament_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Standings>()
                .HasOne(t => t.Team)
                .WithMany(s => s.Standings)
                .HasForeignKey(t => t.Team_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Standings>()
                .HasOne(t => t.Player)
                .WithMany(s => s.Tournament_st)
                .HasForeignKey(t => t.Player_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament_Standings>()
                .HasOne(t => t.Tournament)
                .WithMany(s => s.Standings)
                .HasForeignKey(t => t.Tournament_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Creator)
                .WithMany(s => s.Tournament_owner)
                .HasForeignKey(t => t.Creator_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Creator)
                .WithMany(s => s.Team_owner)
                .HasForeignKey(t => t.Creator_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team_Statistic>()
                .HasOne(t => t.Sport_Statistic)
                .WithMany(s => s.TeamStatistics)
                .HasForeignKey(t => t.Stat_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player_Statistic>()
                .HasOne(p => p.Sport_Statistic)
                .WithMany(s => s.PlayerStatistics)
                .HasForeignKey(p => p.Stat_ID)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Sport_Statistic>().HasData(
                new Sport_Statistic { ID = 1, Sport_ID = 7, Stat_Name = "Голы", DataType = "int" },
                new Sport_Statistic { ID = 2, Sport_ID = 7, Stat_Name = "Пасы", DataType = "int" },
                new Sport_Statistic { ID = 3, Sport_ID = 7, Stat_Name = "Фолы", DataType = "int" },
                new Sport_Statistic { ID = 4, Sport_ID = 7, Stat_Name = "Удары по воротам", DataType = "int" },
                new Sport_Statistic { ID = 5, Sport_ID = 7, Stat_Name = "Удары в створ", DataType = "int" },
                new Sport_Statistic { ID = 6, Sport_ID = 7, Stat_Name = "Офсайды", DataType = "int" },
                new Sport_Statistic { ID = 7, Sport_ID = 7, Stat_Name = "Угловые", DataType = "int" },
                new Sport_Statistic { ID = 8, Sport_ID = 7, Stat_Name = "Карточки (жёлтые)", DataType = "int" },
                new Sport_Statistic { ID = 9, Sport_ID = 7, Stat_Name = "Карточки (красные)", DataType = "int" },
                new Sport_Statistic { ID = 10, Sport_ID = 7, Stat_Name = "Владение мячом (%)", DataType = "float" },
                new Sport_Statistic { ID = 11, Sport_ID = 7, Stat_Name = "Точные передачи (%)", DataType = "float" },

                new Sport_Statistic { ID = 12, Sport_ID = 8, Stat_Name = "Очки", DataType = "int" },
                new Sport_Statistic { ID = 13, Sport_ID = 8, Stat_Name = "Подборы", DataType = "int" },
                new Sport_Statistic { ID = 14, Sport_ID = 8, Stat_Name = "Передачи", DataType = "int" },
                new Sport_Statistic { ID = 15, Sport_ID = 8, Stat_Name = "Блок-шоты", DataType = "int" },
                new Sport_Statistic { ID = 16, Sport_ID = 8, Stat_Name = "Перехваты", DataType = "int" },
                new Sport_Statistic { ID = 17, Sport_ID = 8, Stat_Name = "Потери", DataType = "int" },
                new Sport_Statistic { ID = 18, Sport_ID = 8, Stat_Name = "Фолы", DataType = "int" },
                new Sport_Statistic { ID = 19, Sport_ID = 8, Stat_Name = "3-очковые попадания", DataType = "int" },
                new Sport_Statistic { ID = 20, Sport_ID = 8, Stat_Name = "3-очковые попытки", DataType = "int" },
                new Sport_Statistic { ID = 21, Sport_ID = 8, Stat_Name = "2-очковые попадания", DataType = "int" },
                new Sport_Statistic { ID = 22, Sport_ID = 8, Stat_Name = "2-очковые попытки", DataType = "int" },
                new Sport_Statistic { ID = 23, Sport_ID = 8, Stat_Name = "Штрафные попадания", DataType = "int" },
                new Sport_Statistic { ID = 24, Sport_ID = 8, Stat_Name = "Штрафные попытки", DataType = "int" },
                new Sport_Statistic { ID = 25, Sport_ID = 8, Stat_Name = "Эффективность (%)", DataType = "float" },

                new Sport_Statistic { ID = 26, Sport_ID = 9, Stat_Name = "Эйсы", DataType = "int" },
                new Sport_Statistic { ID = 27, Sport_ID = 9, Stat_Name = "Двойные ошибки", DataType = "int" },
                new Sport_Statistic { ID = 28, Sport_ID = 9, Stat_Name = "Выигранные подачи", DataType = "int" },
                new Sport_Statistic { ID = 29, Sport_ID = 9, Stat_Name = "Процент первой подачи", DataType = "float" },
                new Sport_Statistic { ID = 30, Sport_ID = 9, Stat_Name = "Выигранные розыгрыши", DataType = "int" },
                new Sport_Statistic { ID = 31, Sport_ID = 9, Stat_Name = "Брейк-поинты реализованы", DataType = "int" },
                new Sport_Statistic { ID = 32, Sport_ID = 9, Stat_Name = "Брейк-поинты всего", DataType = "int" },
                new Sport_Statistic { ID = 33, Sport_ID = 9, Stat_Name = "Ошибки (невынужденные)", DataType = "int" },
                new Sport_Statistic { ID = 34, Sport_ID = 9, Stat_Name = "Вынужденные ошибки", DataType = "int" },
                new Sport_Statistic { ID = 35, Sport_ID = 9, Stat_Name = "Процент выигранных очков", DataType = "float" }
);
        }
    }
}

