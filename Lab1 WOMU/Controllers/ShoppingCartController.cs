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
        private TankDatabase db = new TankDatabase();


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
                CartTotal = cart.GetTotal() * 1.25,
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
            var results = new SCconfirmVM
            {
                Message = Server.HtmlEncode(addedItem.ProduktNamn) +
                    " har lagts till i din varukorg",
                CartTotal = cart.GetTotal() * 1.25,
                CartCount = cart.GetCount(),
                ItemCount = 1,
                DeleteId = id,
                totPris = addedItem.Pris * 1
                
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

            // Get the name of the Produkt to display confirmation
            var itemName = db.Produkter
                .Single(item => item.ProduktID == id);


            
            cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new SCconfirmVM
            {
                Message = "Alla " + Server.HtmlEncode(itemName.ProduktNamn) +
                    "'s har tagits bort from varukorgen.",
                CartTotal = cart.GetTotal() * 1.25,
                CartCount = cart.GetCount(),
                ItemCount = 0,
                DeleteId = id
                
            };
            return Json(results);
        }
        /// <summary>
        /// adds One CartItem to existing Cartitem in cart. then calculates new values
        /// </summary>
        /// <param name="id">
        ///  ID of CartItem in Cart
        /// </param>
        /// <returns>
        /// return New values for values in CartItem
        /// </returns>
        [HttpPost]
        public ActionResult CountP(int id)
        {
            var Temp = db.CartItem.Single(
               Temp1 => Temp1.CartItemID == id);

            Temp.Count = Temp.Count + 1;
            Temp.totPris = Temp.Produkt.Pris * Temp.Count;
            db.SaveChanges();

            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (Temp.Count <= Temp.Produkt.AntalILager)
            {
                var results = new CountModelView
                {
                    Message = Temp.Produkt.ProduktNamn + " har ändrats till " + Temp.Count + " stcken.",
                    ItemCount = Temp.Count,
                    TotPris = Temp.totPris,
                    CartTotal = cart.GetTotal() * 1.25,
                    CartCount = cart.GetCount(),
                    ItemID = Temp.ProduktID
                    
                    
                };
                db.SaveChanges();
                return Json(results);
            }
            else
            {
                Temp.Count = Temp.Count - 1;
                Temp.totPris = Temp.Produkt.Pris * Temp.Count;
                db.SaveChanges();
                var results = new CountModelView
                {
                    Message = "Inga mer Produkter i Lager.",
                    ItemCount = Temp.Count,
                    TotPris = Temp.totPris,
                    CartTotal = cart.GetTotal() * 1.25,
                    CartCount = cart.GetCount(),
                    ItemID = Temp.ProduktID
                  
                    
                };
                db.SaveChanges();
                return Json(results);
            }


        }
        /// <summary>
        /// removes One CartItem to existing Cartitem in cart. then calculates new values
        /// </summary>
        /// <param name="id"> 
        /// ID of CartItem in Cart
        /// </param>
        /// <returns>
        /// return New values for values in CartItem
        /// </returns> 
        public ActionResult CountM(int id)
        {
            {
                var Temp = db.CartItem.Single(
                Temp1 => Temp1.CartItemID == id);

                Temp.Count = Temp.Count - 1;
                Temp.totPris = Temp.Produkt.Pris * Temp.Count;
                db.SaveChanges();

                
                var cart = ShoppingCart.GetCart(this.HttpContext);

                if (Temp.Count > 0)
                {
                    // Display the confirmation message
                    var results = new CountModelView
                    {
                        Message = ("En " + Temp.Produkt.ProduktNamn + " har tagits bort från din varukorg."),
                        ItemCount = Temp.Count,
                        CartTotal = cart.GetTotal() * 1.25,
                        CartCount = cart.GetCount(),
                        ItemID = Temp.ProduktID,
                        TotPris = getCartItemTotalPris(Temp)
                    };
                    db.SaveChanges();
                    return Json(results);
                }
                else
                {
                    Temp.Count = Temp.Count + 1;
                    Temp.totPris = Temp.Produkt.Pris * Temp.Count;
                    db.SaveChanges();
                    cart.RemoveFromCart(Temp.ProduktID); 
                    var results = new CountModelView
                    {
                        Message = (Temp.Produkt.ProduktNamn + " har tagits bort från din varukorg."),
                        CartTotal = cart.GetTotal() * 1.25,
                        CartCount = cart.GetCount(),
                        ItemID = Temp.ProduktID,
                        TotPris = Temp.totPris
                    };
                    db.SaveChanges();
                    return Json(results);
                }
            }
            }
        public int getCartItemTotalPris(CartItem item)
        {
            var pris = item.Count * item.Produkt.Pris;
            
            return (pris);
        }
    }
}