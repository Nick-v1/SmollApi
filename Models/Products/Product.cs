using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public String listingID { get; set; }
        public int userID { get; set; }
        public DateTime registration { get; set; }
        public Decimal price { get; set; }
    }
}
