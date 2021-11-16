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
        int Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string AccounType { get; set; }
        bool verified { get; set; }
    }
}
