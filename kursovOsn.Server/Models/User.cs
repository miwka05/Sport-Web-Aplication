namespace kursovOsn.Server.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // Здесь можно использовать хэширование пароля
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public string Sex { get; set; }
    }
}
