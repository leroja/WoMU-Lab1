using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models
{
    public class OrderRad
    {
        [Key]
        [Column(Order = 1)]
        public int ProduktID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int OrderID { get; set; }
        public int Antal { get; set; }


        public virtual Order Order { get; set; }
        public virtual Produkt Produkt { get; set; }
    }
}