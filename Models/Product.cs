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
        [Key]
        string ListingId { get; set; }
        int ProductId { get; set; }
        int UserId { get; set; }
        DateTime registration { get; set; }
        decimal price { get; set; }
    }
}
