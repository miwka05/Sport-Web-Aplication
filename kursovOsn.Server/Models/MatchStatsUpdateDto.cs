namespace kursovOsn.Server.Models
{
    public class MatchStatsUpdateDto
    {
        public List<TeamStatDto> TeamStats { get; set; }
        public List<PlayerStatDto> PlayerStats { get; set; }
    }

    public class TeamStatDto
    {
        public int TeamId { get; set; }
        public int StatId { get; set; }
        public int Value { get; set; }
    }

    public class PlayerStatDto
    {
        public string PlayerId { get; set; }
        public int StatId { get; set; }
        public int Value { get; set; }
    }
}
