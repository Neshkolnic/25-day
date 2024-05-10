using System.Collections.Generic;
using System;
using System.Data.Entity;

namespace ConfectioneryApp
{
    public class ConfectioneryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public ConfectioneryDbContext() : base("name=WindowsFormsApp1.Properties.Settings.ConfectioneryDBBConnectionString")
        {
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } // Изменили тип на decimal
        public string Category { get; set; }
    }


    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
