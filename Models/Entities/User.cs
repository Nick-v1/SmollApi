using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
