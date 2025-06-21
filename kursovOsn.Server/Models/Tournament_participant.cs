namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Tournament_participant
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Tournament_ID { get; set; }

        public string? User_ID { get; set; } // для одиночных участников

        public int? Team_ID { get; set; } // для командных

        [ForeignKey("Tournament_ID")]
        public Tournament Tournament { get; set; }

        [ForeignKey("User_ID")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("Team_ID")]
        public Team? Team { get; set; }
    }

}
