using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Product
    {
        public int Id { get; set; } //PK
        public DateTime Registration { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int PhoneId { get; set; }//FK
        public Phone Phone { get; set; }//nav
    }
}
