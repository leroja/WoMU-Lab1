using Lab1_WOMU.Models;
using Lab1_WOMU.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lab1_WOMU.Controllers
{
    public class ShoppingCartController : Controller
    {
        private DatabaseTest1 db = new DatabaseTest1();


        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartVM
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                RelatedProdukts = cart.GetRelatedProdukts()
            };

            return View(viewModel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        // GET: Produkt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produkt produkt = db.Produkter.Find(id);
            if (produkt == null)
            {
                return HttpNotFound();
            }
            return View(produkt);
        }



        /// <summary>
        /// adds a product to the cart
        /// </summary>
        /// <param name="id">
        /// id of product that is going to be added to the cart
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        // GET: /AddToCart/5
        [HttpPost]
        public ActionResult AddToCart(int id)
        {

            var addedItem = db.Produkter
                .Single(item => item.ProduktID == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            // Display the confirmation message
            var results = new SCremoveVM
            {
                Message = Server.HtmlEncode(addedItem.ProduktNamn) +
                    " har lagts till i din varukorg",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = 0,
                DeleteId = id
            };
            return Json(results);
        }

        /// <summary>
        /// removes a specified product
        /// </summary>
        /// <param name="id">
        /// id of product to be removed
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            
            
            var cart = ShoppingCart.GetCart(this.HttpContext);


            string itemName = db.Produkter
                .Single(item => item.ProduktID == id).ProduktNamn;

            
            cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new SCremoveVM
            {
                Message = Server.HtmlEncode(itemName) +
                    " har tagits bort from varukorgen.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = 0,
                DeleteId = id
            };
            return Json(results);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 
        /// </returns>
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
            if (Temp.Count <= Temp.Produkt.AntalILager)
            {
                var results = new CountModelView
                {
                    Message = Temp.Produkt.ProduktNamn + " har ändrats till " + Temp.Count + " stcken.",
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
                    Message = "Inga mer Produkter i Lager.",
                    ItemCount = Temp2.Count,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemID = Temp2.ProduktID
                };
                
                return Json(results);
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CountM(int id)
        {
            {
                var Temp = db.CartItem.Single(
                Temp1 => Temp1.CartItemID == id);

                var Temp2 = db.CartItem.Single(
                   Temp1 => Temp1.CartItemID == id);

                Temp.Count = Temp.Count - 1;
                db.SaveChanges();

                
                var cart = ShoppingCart.GetCart(this.HttpContext);

                if (Temp.Count > 0)
                {
                    // Display the confirmation message
                    var results = new CountModelView
                    {
                        Message = ("en " + Temp.Produkt.ProduktNamn + " har tagits bort från din varukorg."),
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
                    cart.RemoveFromCart(Temp.ProduktID); 
                    var results = new CountModelView
                    {
                        Message = (Temp.Produkt.ProduktNamn + " har tagits bort från din varukorg."),
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