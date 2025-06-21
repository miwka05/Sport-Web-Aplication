using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace kursovOsn.Server.Models
{
    public class ApplicationUser:IdentityUser
    {
        // Дополнительные свойства
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public DateTime DateOfBirth { get; set; }
        public string Sex {  get; set; }
        public bool? admin { get; set; }
        public bool? ban { get; set; }
        public string? reasonBan { get; set; } = string.Empty;
        // Связи
        [ForeignKey("Sport")]
        public int? Sport_ID { get; set; }
        public Sport? Sport { get; set; }
        public ICollection<Team_player>? Teams { get; set; }
        public ICollection<Team_entry>? Team_e { get; set; }
        public ICollection<Tournament_Entry>? Tournaments { get; set; }
        public ICollection<Tournament_participant>? Tournament_p { get; set; }
        public ICollection<Match>? Match1 { get; set; }
        public ICollection<Match>? Match2 { get; set; }
        public ICollection<Tournament_Standings>? Tournament_st { get; set; }
        public ICollection<Player_Statistic>? Stat { get; set; }
        public ICollection<Team>? Team_owner { get; set; }
        public ICollection<Tournament>? Tournament_owner { get; set; }
        // Метод для получения полного имени пользователя (например, для отображения)
        public string FullName => $"{FirstName} {LastName}";
    }
}
