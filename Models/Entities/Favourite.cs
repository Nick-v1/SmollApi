using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Favourite
    {
        public int Id { get; set; } //primary key
       
        public int UserId { get; set; } //foreign key

        public User User { get; set; }  //navigation property
        public int PhoneId { get; set; } //foreign key
        public Phone Phone { get; set; } //navigation property
        
    }
}