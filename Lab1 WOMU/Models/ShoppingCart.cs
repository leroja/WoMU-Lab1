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







        public int ChangeProduktCount(Produkt Produkt, int count)
        {
            // Get the matching cart and item instances
            var cartItem = db.CartItem.SingleOrDefault(
                c => c.CartId == ShoppingCartID
                && c.ProduktID == Produkt.ProduktID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    CartItemID = Produkt.ProduktID,
                    CartId = ShoppingCartID,
                    Count = count,
                    DateCreated = DateTime.Now
                };
                db.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Count = count;
            }
            // Save changes
            db.SaveChanges();

            return cartItem.Count;
        }



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

        public int AddToCart(Produkt Produkt, int count)
        {
            // Get the matching cart and item instances
            var cartItem = db.CartItem.SingleOrDefault(
                c => c.CartId == ShoppingCartID
                && c.ProduktID == Produkt.ProduktID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    CartItemID = Produkt.ProduktID,
                    CartId = ShoppingCartID,
                    Count = count,
                    DateCreated = DateTime.Now
                };
                db.CartItem.Add(cartItem);
            }
            else
            {
                //    // If the item does exist in the cart, 
                //    // then uppdate the quantity
                cartItem.Count = count;
            }
        // Save changes
        db.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {


            // Get the cart

            var cartItem = db.CartItem.Single(
                cart => cart.CartId == ShoppingCartID
                && cart.ProduktID == id);


            int itemCount = 0;

            if (cartItem != null)
            {
                db.CartItem.Remove(cartItem);
                //Save changes
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
            // Save changes
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
            //int total = (from cartItems in db.CartItem
            //                  where cartItems.CartId == ShoppingCartID
            //                  select (int)cartItems.Count *
            //                  cartItems.Produkt.Pris).Sum();

            //return total;
            return Convert.ToInt32( total ?? decimal.Zero);
        }

        public Order CreateOrder(Order order)
        {
            int orderTotal = 0;
            order.OrderRader = new List<OrderRad>();

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {

                if (item.Count < item.Produkt.AntalILager)
                {
                    var orderDetail = new OrderRad
                    {

                        ProduktID = item.ProduktID,
                        OrderID = order.OrderID,
                        TotalPris = item.Produkt.Pris * item.Count,
                        Antal = item.Count
                    };

                    // Set the order total of the shopping cart
                    orderTotal += (item.Count * item.Produkt.Pris);
                    order.OrderRader.Add(orderDetail);
                    db.OrderRad.Add(orderDetail);
                }
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order;
        }

        // We're using HttpContextBase to allow access to cookies.
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
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
    }

}