using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab1_WOMU.Models;

namespace Lab1_WOMU.Controllers
{
    public class OrderController : Controller
    {
        private DatabaseTest1 db = new DatabaseTest1();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// the view
        /// </returns>
        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// takes in an unfinished order and finish it, and hen saves it to the database and clears the cart. 
        /// </summary>
        /// <param name="order">
        /// the order that is going to be created
        /// </param>
        /// <returns>
        /// redirect to Order completion page
        /// </returns>
        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    order.OrderDate = DateTime.Now;

                    var cart = ShoppingCart.GetCart(this.HttpContext);

                    order.OrderRader = new List<OrderRad>();
                    order.Total = 0;
                    foreach (var item in cart.GetCartItems())
                    {
                        var prod = db.Produkter.Where(a => a.ProduktID == item.ProduktID).Single();

                        if (item.Count < prod.AntalILager)
                        {
                            var orderRad = new OrderRad
                            {

                                ProduktID = item.ProduktID,
                                OrderID = order.OrderID,
                                TotalPris = item.Produkt.Pris * item.Count,
                                Antal = item.Count
                            };

                            prod.AntalILager -= item.Count;
                            

                            order.Total += orderRad.TotalPris;
                            order.OrderRader.Add(orderRad);
                            
                        }
                    }
                    db.Order.Add(order);
                    order.OrderRader.Count();

                    db.SaveChanges();
                    cart.EmptyCart();
                    return RedirectToAction("Completed", "Order", new { id = order.OrderID });
                }
            }

            return View(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">
        /// Id of the completed order
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public ActionResult Completed(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        
        /// <summary>
        /// finds all the orderLines that belong the order if the serchstring is valid
        /// else it return no orderLines
        /// </summary>
        /// <param name="SearchString">
        /// user search input
        /// </param>
        /// <returns>
        /// returns all orderRader that belongs to the sought after order
        /// or none if the searchstring is empty or not an order
        /// </returns>
        public ActionResult OrderKoll(string SearchString)
        {
            var order = db.OrderRad.Include(c => c.Produkt);

            int temp = 0;

            if ((!String.IsNullOrEmpty(SearchString)) && int.TryParse(SearchString, out temp))
            {
                order = order.Where(s => s.Order.OrderID.Equals(temp));
            }
            else {
                order = order.Where(s => s.Order.OrderID.Equals(0));
            }
            return View(order.ToList());
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
        public ActionResult OrderItemDetail(int? id)
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
