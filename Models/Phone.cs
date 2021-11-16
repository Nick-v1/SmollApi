using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class Phone
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Manifacturer { get; set; }
        public int RAM { get; set; }
        public int ROM { get; set; }
        public double ScreenSize { get; set; }
        public string OS { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
