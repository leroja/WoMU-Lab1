using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models
{
    public class CartItem
    {

        public int CartItemID { get; set; }
        public string CartId { get; set; }
        public int ProduktID { get; set; }
        public int Count { get; set; }
        public int totPris { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Produkt Produkt { get; set; }
    }
}