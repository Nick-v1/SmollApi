using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Phones
    {
        
        public int id { get; set; }
        public string manifacturer { get; set; }
        public int RAM { get; set; }
        public int ROM { get; set; }
        public decimal ScreenSize { get; set; }
        public string OS { get; set; }
    }
}
