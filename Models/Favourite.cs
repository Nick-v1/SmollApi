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
        [Key] public int UserId { get; set; }
        public int PhonesId { get; set; }
    }
}