namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Team_player
    {
        [Key]
        public int Id { get; set; }

        public string ID_User { get; set; }
        public ApplicationUser User { get; set; }

        public int ID_Team { get; set; }
        public Team Team { get; set; }

        public string Role { get; set; }
    }

}
