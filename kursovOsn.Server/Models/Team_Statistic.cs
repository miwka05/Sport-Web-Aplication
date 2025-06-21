namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Team_Statistic
    {
        [Key]
        public int Id { get; set; }

        public int Match_ID { get; set; }
        public Match Match { get; set; }

        public int Team_ID { get; set; }
        public Team Team { get; set; }

        public int Stats { get; set; }

        public int Stat_ID { get; set; }
        public Sport_Statistic Sport_Statistic { get; set; }
    }

}
