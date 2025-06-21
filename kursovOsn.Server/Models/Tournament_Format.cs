using System.ComponentModel.DataAnnotations;

namespace kursovOsn.Server.Models
{
    public class Tournament_Format
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Tournament>? Tournaments { get; set; }
    }
}

