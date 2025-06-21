namespace kursovOsn.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Play_Off
    {
        [Key]
        public int Id { get; set; }

        public int Match_ID { get; set; }
        public Match Match { get; set; }

        public int? Next_Match_Winner_ID { get; set; }
        public Match? NextMatchWinner { get; set; }

        public int? Next_Match_Loser_ID { get; set; }
        public Match? NextMatchLoser { get; set; }
    }

}
