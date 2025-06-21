namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Tournament_Standings
    {
        [Key]
        public int Id { get; set; }

        public int Tournament_ID { get; set; }
        public Tournament Tournament { get; set; }

        public int? Team_ID { get; set; }
        public Team? Team { get; set; }

        public string? Player_ID { get; set; }
        public ApplicationUser? Player { get; set; }

        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Points { get; set; }
        public int Goals_Scored { get; set; }
        public int Goals_Conceded { get; set; }
    }

}
