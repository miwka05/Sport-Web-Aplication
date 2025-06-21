namespace kursovOsn.Server.Models
{
    public class CreateMatchWithStageDto
    {
        public int Tournament_ID { get; set; }
        public int Team1_ID { get; set; }
        public int Team2_ID { get; set; }
        public string Data { get; set; }     // формат: yyyy-MM-dd
        public string Time { get; set; }     // формат: HH:mm
        public string Stage_Name { get; set; }
        public int Stage_Order { get; set; }
    }
}
