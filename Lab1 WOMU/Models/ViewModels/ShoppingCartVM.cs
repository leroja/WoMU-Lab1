using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models.ViewModels
{
    public class ShoppingCartVM
    {
        [Key]
        public List<CartItem> CartItems { get; set; }
        public int CartTotal { get; set; }

        public List<Produkt> RelatedProdukts { get; set; }
    }
}