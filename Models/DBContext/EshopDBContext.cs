using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public class EshopDBContext : DbContext
    {
        public EshopDBContext(DbContextOptions<EshopDBContext> options) : base(options)
        {   
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasNoKey();
            modelBuilder.Entity<Product>().HasNoKey();
        }

        public DbSet<Phone> Phones { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Ban> Bans { get; set; }
    }
}
