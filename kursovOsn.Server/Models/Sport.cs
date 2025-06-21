namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Sport
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Tournament>? Tournaments { get; set; }
        public ICollection<Team>? Teams { get; set; }
        public ICollection<ApplicationUser>? Users { get; set; }
        public ICollection<Match>? Matches { get; set; }
        public ICollection<Sport_Statistic>? Statistics { get; set; }
    }

}
