using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Lab1_WOMU.Models
{
    public class Produkt
    {
        public int ProduktID { get; set; }
        public string ProduktNamn { get; set; }
        public string ProduktBeskrivning { get; set; }
        public int Pris { get; set; }
        public int AntalILager { get; set; }
    }

    public class DatabaseTest : DbContext
    {
        public DbSet<Produkt> Produkter { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderRad> OrderRad { get; set; }
        public DbSet<Kund> Kund { get; set; }
    }
}