﻿namespace kursovOsn.Server.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; } // Здесь можно использовать хэширование пароля
    }
}
