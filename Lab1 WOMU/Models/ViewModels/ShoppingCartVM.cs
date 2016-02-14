using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models.ViewModels
{
    public class ShoppingCartVM
    {
        
        public List<CartItem> CartItems { get; set; }
        public double CartTotal { get; set; }

        public List<Produkt> RelatedProdukts { get; set; }
    }
}