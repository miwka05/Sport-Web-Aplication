namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Sport_ID { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string? Creator_ID { get; set; }
        public bool? ban { get; set; }
        public string? reasonBan { get; set; } = string.Empty;

        public Sport? Sport { get; set; }
        public ApplicationUser? Creator { get; set; }
        public ICollection<Tournament_Entry>? TournamentEntries { get; set; }
        public ICollection<Tournament_participant>? TournamentParticipants { get; set; }
        public ICollection<Team_entry>? TeamEntries { get; set; }
        public ICollection<Team_player>? TeamPlayers { get; set; }
        public ICollection<Match>? MatchesAsTeam1 { get; set; }
        public ICollection<Match>? MatchesAsTeam2 { get; set; }
        public ICollection<Tournament_Standings>? Standings { get; set; }
        public ICollection<Team_Statistic>? Statistics { get; set; }
    }

}
