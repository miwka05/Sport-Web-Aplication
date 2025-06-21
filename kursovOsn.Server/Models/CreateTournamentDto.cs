namespace kursovOsn.Server.Models
{
    public class CreateTournamentDto
    {
        public string Name { get; set; }
        public int Sport_ID { get; set; }
        public string Adress { get; set; }
        public string Age { get; set; }
        public string Info { get; set; }
        public string Pol { get; set; }
        public bool Solo { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Format_ID { get; set; }
    }
}
