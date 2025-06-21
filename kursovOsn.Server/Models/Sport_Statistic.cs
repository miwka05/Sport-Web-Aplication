namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Sport_Statistic
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Sport_ID { get; set; }

        [Required]
        public string Stat_Name { get; set; }
        public string DataType { get; set; }

        [ForeignKey("Sport_ID")]
        public Sport Sport { get; set; }

        public ICollection<Team_Statistic>? TeamStatistics { get; set; }
        public ICollection<Player_Statistic>? PlayerStatistics { get; set; }
    }

}
