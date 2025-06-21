namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Player_Statistic
    {
        [Key]
        public int Id { get; set; }

        public int Match_ID { get; set; }
        public Match Match { get; set; }

        public string Player_ID { get; set; }
        public ApplicationUser Player { get; set; }

        public int Stats { get; set; }
        public int Stat_ID { get; set; }
        public Sport_Statistic Sport_Statistic { get; set; }
    }

}
