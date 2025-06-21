namespace kursovOsn.Server.Models
{
    public class MatchStatisticDto
    {
        public int MatchId { get; set; }

        public bool IsTeam { get; set; } 
        public int? TeamId { get; set; }
        public string? PlayerId { get; set; }

        public List<StatValueDto> Statistics { get; set; }
    }

    public class StatValueDto
    {
        public int StatId { get; set; }
        public int Value { get; set; } 
    }
}
