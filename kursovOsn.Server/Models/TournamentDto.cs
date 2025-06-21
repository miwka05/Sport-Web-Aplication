namespace kursovOsn.Server.Models
{
    public class TournamentDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SportName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Adress { get; set; }
        public string Age { get; set; }
        public string Info { get; set; }
        public string Pol { get; set; }
        public string Status { get; set; }
        public bool? Solo { get; set; }
        public bool? Ban { get; set; }
        public string? reasonBan { get; set; } = string.Empty;
    }
}
