namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team_entry
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Team_ID { get; set; }

        [Required]
        public string User_ID { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        [ForeignKey("Team_ID")]
        public Team Team { get; set; }

        [ForeignKey("User_ID")]
        public ApplicationUser User { get; set; }
    }

}
