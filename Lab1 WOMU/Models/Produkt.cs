using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lab1_WOMU.Models
{
    public class Produkt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProduktID { get; set; }
        [Required]
        public string ProduktNamn { get; set; }
        [Required]
        public string ProduktBeskrivning { get; set; }
        public int Pris { get; set; }
        public int AntalILager { get; set; }
    }

    public class DatabaseTest1 : DbContext
    {
        public DbSet<Produkt> Produkter { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderRad> OrderRad { get; set; }
        public DbSet<Kund> Kund { get; set; }


    }
}