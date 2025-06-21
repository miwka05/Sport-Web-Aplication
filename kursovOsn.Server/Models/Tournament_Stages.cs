using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kursovOsn.Server.Models
{
    public class Tournament_Stages
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Tournament")]
        public int Tournament_ID { get; set; }
        public Tournament Tournament { get; set; }

        [Required]
        public string Name { get; set; }

        public int Stage_order { get; set; }

        public ICollection<Match>? Matches { get; set; }
    }
}
