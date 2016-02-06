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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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


        /// <summary>
        /// adds the specified product to the cart
        /// </summary>
        /// <param name="Produkt">
        /// id of the product 
        /// </param>
        public void AddToCart(Produkt Produkt)
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
        
            db.SaveChanges();
        }


        /// <summary>
        /// removes the specified product from the cart
        /// </summary>
        /// <param name="id">
        /// id of the product 
        /// </param>
        public void RemoveFromCart(int id)
        {

            var cartItem = db.CartItem.Single(
                cart => cart.CartId == ShoppingCartID
                && cart.ProduktID == id);


            if (cartItem != null)
            {
                db.CartItem.Remove(cartItem);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Empties the cart
        /// </summary>
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


        /// <summary>
        /// return a list of all cartItems that belong to the cart
        /// </summary>
        /// <returns>
        /// a list of cartItems
        /// </returns>
        public List<CartItem> GetCartItems()
        {
            return db.CartItem.Where(
                cart => cart.CartId == ShoppingCartID).ToList();
        }

        /// <summary>
        /// this function gets the count of all item and sums them
        /// </summary>
        /// <returns>
        /// the sum of all items in the cart
        /// 0 if all entries are null
        /// </returns>
        public int GetCount()
        {
            
            int? count = (from cartItems in db.CartItem
                          where cartItems.CartId == ShoppingCartID
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }


        /// <summary>
        /// sums up the total price of the cart
        /// </summary>
        /// <returns>
        /// the total price of the cart
        /// </returns>
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

        /// <summary>
        /// creates a list of of related products
        /// </summary>
        /// <returns>
        /// a list of all related products to the products in the cart
        /// </returns>
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

            related = related.Distinct().ToList();

            foreach(var i in cart)
            {
                related.Remove(i.Produkt);
            }

            return related;
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