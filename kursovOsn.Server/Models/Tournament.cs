using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition;

namespace kursovOsn.Server.Models
{
    public class Tournament
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Sport")]
        public int Sport_ID { get; set; }
        public Sport Sport { get; set; }

        [ForeignKey("Creator")]
        public string? Creator_ID { get; set; }
        public ApplicationUser? Creator { get; set; }

        public string Adress { get; set; }
        public string Age { get; set; }
        public string Info { get; set; }
        public string Pol { get; set; }
        public string Status { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool? solo { get; set; }

        public bool? ban { get; set; }
        public string? reasonBan { get; set; } = string.Empty;

        [ForeignKey("Format")]
        public int Format_ID { get; set; }
        public Tournament_Format Format { get; set; }

        public ICollection<Tournament_Stages>? Stages { get; set; }
        public ICollection<Match>? Matches { get; set; }
        public ICollection<Tournament_Entry>? Entries { get; set; }
        public ICollection<Tournament_participant>? Participants { get; set; }
        public ICollection<Tournament_Standings>? Standings { get; set; }
    }
}
