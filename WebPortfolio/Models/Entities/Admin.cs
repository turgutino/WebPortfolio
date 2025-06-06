﻿using System.ComponentModel.DataAnnotations;

namespace WebPortfolio.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
