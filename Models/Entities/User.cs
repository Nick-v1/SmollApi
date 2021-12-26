﻿using System.ComponentModel.DataAnnotations;

namespace SmollApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string AccounType { get; set; }
        
        [Required]
        public byte Verified { get; set; }
        public string? Status { get; set; }
    }
}
