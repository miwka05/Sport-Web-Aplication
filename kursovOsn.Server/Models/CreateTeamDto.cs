namespace kursovOsn.Server.Models
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public int? Sport_ID { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
    }
}
