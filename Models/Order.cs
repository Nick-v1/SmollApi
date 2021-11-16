using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Order
    {
        string ListingId { get; set; }
        int UserId { get; set; }
        DateTime OrderDate { get; set; }
    }
}
