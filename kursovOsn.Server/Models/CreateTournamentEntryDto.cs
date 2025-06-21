namespace kursovOsn.Server.Models
{
    public class CreateTournamentEntryDto
    {
        public int TournamentId { get; set; }
        public string Info { get; set; }
        public int? TeamId { get; set; } // Добавляем поле для заявки от команды
    }
}
