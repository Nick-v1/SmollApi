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
        [Key]
        public int Id { get; private set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AccounType { get; private set; }
        public byte Verified { get; private set; }

        public void SetVerified(byte verbyte)
        {
            Verified = verbyte;
        }

        public void SetAccounType(string type)
        {
            AccounType = type;
        }
        public void SetId(int id)
        {
            Id = id;
        }
    }
}
