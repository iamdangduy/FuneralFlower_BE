﻿namespace FuneralFlower_BE.Models
{
    public class User
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
