using kursovOsn.Server.Models;
using System.ComponentModel.DataAnnotations;

public class Tournament_Entry
{
    [Key]
    public int ID { get; set; }

    public string? ID_User { get; set; }
    public ApplicationUser? User { get; set; }

    public int ID_Tournament { get; set; }
    public Tournament Tournament { get; set; }

    public int? ID_Team { get; set; }
    public Team? Team { get; set; }

    public string Info { get; set; }

    [Required]
    public string Status { get; set; } = "Pending"; 
}


