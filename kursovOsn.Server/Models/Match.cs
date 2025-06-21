using System.ComponentModel.DataAnnotations;

namespace kursovOsn.Server.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Player1_ID { get; set; }
        public string? Player2_ID { get; set; }
        public int? Team1_ID { get; set; }
        public int? Team2_ID { get; set; }
        public int Sport_ID { get; set; }
        public int Tournament_ID { get; set; }
        public int Stage_ID { get; set; }
        public TimeSpan Time { get; set; }
        public int? Team1_Score { get; set; }
        public int? Team2_Score { get; set; }
        public string? Winner { get; set; }
        public Sport Sport { get; set; }
        public Team? Team1 { get; set; }
        public Team? Team2 { get; set; }
        public Tournament Tournament { get; set; }
        public Tournament_Stages Stage { get; set; }
        public ApplicationUser? Player1 { get; set; }
        public ApplicationUser? Player2 { get; set; }

        public ICollection<Team_Statistic> TeamStat { get; set; }
        public ICollection<Player_Statistic> PlStat { get; set; }
        public ICollection<Play_Off> Play_Offs { get; set; }
        public ICollection<Play_Off> Play_Offs_W { get; set; }
        public ICollection<Play_Off> Play_Offs_L { get; set; }
    }

}
