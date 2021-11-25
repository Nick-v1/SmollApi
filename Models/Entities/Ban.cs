using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Ban
    {
        public int Id { get; set; }
        public DateTime BannedDate { get; set; }
        public string? Reason { get; set; }
        public string Action { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
