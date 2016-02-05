using Lab1_WOMU.Models;
using Lab1_WOMU.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1_WOMU.Controllers
{
    public class ShoppingCartController : Controller
    {
        private DatabaseTest1 db = new DatabaseTest1();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartVM
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            Console.WriteLine("afsdfsdgsdg");

            // Retrieve the item from the database
            var addedItem = db.Produkter
                .Single(item => item.ProduktID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            int count = cart.AddToCart(addedItem);

            // Display the confirmation message
            var results = new SCremoveVM
            {
                Message = Server.HtmlEncode(addedItem.ProduktNamn) +
                    " has been added to your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = count,
                DeleteId = id
            };
            return Json(results);

            // Go back to the main store page for more shopping
            // return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the Produkt to display confirmation
            string itemName = db.Produkter
                .Single(item => item.ProduktID == id).ProduktNamn;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new SCremoveVM
            {
                Message = "One (1) " + Server.HtmlEncode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
        [HttpPost]
        public ActionResult CountP(int id)
        {
            var Temp = db.CartItem.Single(
               Temp1 => Temp1.CartItemID == id);

            var Temp2 = db.CartItem.Single(
               Temp1 => Temp1.CartItemID == id);

            Temp.Count = Temp.Count + 1;
            db.SaveChanges();

            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (Temp.Count < Temp.Produkt.AntalILager)
            {
                var results = new CountModelView
                {
                    Message = Temp.Produkt.ProduktNamn + "has been change to" + Temp.Count + "st",
                    ItemCount = Temp.Count,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemID = Temp.ProduktID
                };
                db.SaveChanges();
                return Json(results);
            }
            else
            {
                Temp.Count = Temp.Count - 1;
                db.SaveChanges();
                var results = new CountModelView
                {
                    Message = "Inga mer Produkter i Lager",
                    ItemCount = Temp2.Count,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemID = Temp2.ProduktID
                };
                
                return Json(results);
            }


        }
        public ActionResult CountM(int id)
        {
            {
                var Temp = db.CartItem.Single(
                Temp1 => Temp1.CartItemID == id);

                var Temp2 = db.CartItem.Single(
                   Temp1 => Temp1.CartItemID == id);

                Temp.Count = Temp.Count - 1;
                db.SaveChanges();

                // Remove the item from the cart
                var cart = ShoppingCart.GetCart(this.HttpContext);

                if (Temp.Count > 0)
                {
                    // Display the confirmation message
                    var results = new CountModelView
                    {
                        Message = ("en" + Temp.Produkt.ProduktNamn + " har tagits bort från din kundkorg"),
                        ItemCount = Temp.Count,
                        CartTotal = cart.GetTotal(),
                        CartCount = cart.GetCount(),
                        ItemID = Temp.ProduktID
                    };
                    db.SaveChanges();
                    return Json(results);
                }
                else
                {
                    Temp.Count = Temp.Count = 0;
                    db.SaveChanges();
                    var ItemCount = cart.RemoveFromCart(Temp.ProduktID); 
                    var results = new CountModelView
                    {
                        Message = (Temp.Produkt.ProduktNamn + " har tagits bort från din kundvagn"),
                        CartTotal = cart.GetTotal(),
                        CartCount = cart.GetCount(),
                        ItemID = Temp.ProduktID
                    };
                    return Json(results);

                }

            }


        }
    }
}