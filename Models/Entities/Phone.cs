﻿using System.ComponentModel.DataAnnotations;

namespace SmollApi.Models
{
    public class Phone
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manifacturer { get; set; }
        public int RAM { get; set; }
        public int ROM { get; set; }
        public double ScreenSize { get; set; }
        [Required]
        public string OS { get; set; }
    }
}
