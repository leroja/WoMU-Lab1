using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1_WOMU.Models
{
    public class ShoppingCart
    {

        DatabaseTest1 db = new DatabaseTest1();
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ShoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        public virtual ICollection<CartItem> CartItem { get; set; }



        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(Produkt Produkt)
        {
            var cartItem = db.CartItem.SingleOrDefault(
                c => c.CartId == ShoppingCartID
                && c.ProduktID == Produkt.ProduktID);

            if (cartItem == null) // if no cartItem exists, create a new one
            {
                cartItem = new CartItem
                {
                    ProduktID = Produkt.ProduktID,
                    CartId = ShoppingCartID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.CartItem.Add(cartItem);
            }
            else
            {
                //    // If the item does exist in the cart, 
                //    // then uppdate the quantity
                cartItem.Count = cartItem.Count + 1;
            }
        
        db.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {

            var cartItem = db.CartItem.Single(
                cart => cart.CartId == ShoppingCartID
                && cart.ProduktID == id);


            int itemCount = 0;

            if (cartItem != null)
            {
                db.CartItem.Remove(cartItem);

                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.CartItem.Where(
                cart => cart.CartId == ShoppingCartID);

            foreach (var cartItem in cartItems)
            {
                db.CartItem.Remove(cartItem);
            }
    
            db.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return db.CartItem.Where(
                cart => cart.CartId == ShoppingCartID).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.CartItem
                          where cartItems.CartId == ShoppingCartID
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public int GetTotal()
        {
            // Multiply item price by count of that item to get 
            // the current price for each of those items in the cart
            // sum all item price totals to get the cart total

            decimal? total = (from cartItems in db.CartItem
                              where cartItems.CartId == ShoppingCartID
                              select (int?)cartItems.Count *
                              cartItems.Produkt.Pris).Sum();
            
            return Convert.ToInt32(total ?? decimal.Zero);
        }


        public List<Produkt> GetRelatedProdukts()
        {
            List<Produkt> related = new List<Produkt>();

            var cart = GetCartItems();

            List<int> orderIDs = new List<int>();

            foreach (var cartItem in cart)
            {
                var temp = db.OrderRad.Where(o => o.ProduktID == cartItem.ProduktID);

                var temp2 = temp.ToList();

                foreach(var t in temp2)
                {
                    orderIDs.Add(t.OrderID);
                }    
            }

            foreach(var ord in orderIDs)
            {
                var order = db.Order.Where(m => m.OrderID == ord);

                foreach(var o in order)
                {
                    foreach(var rad in o.OrderRader)
                    {
                        related.Add(rad.Produkt);
                    }
                }

            }

            return related.Distinct().ToList();
        }


        
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    
                    Guid tempCartId = Guid.NewGuid();

                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
    }

}