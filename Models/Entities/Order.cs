using System;

namespace SmollApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public int ProductId { get; set; } //FK
        public Product Product { get; set; } //nav
        public int UserId { get; set; } //FK
        public User User { get; set; } //nav
    }
}
