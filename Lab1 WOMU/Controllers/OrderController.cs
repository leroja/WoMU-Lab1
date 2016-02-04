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

        // GET: Order
        public ActionResult Index()
        {
            return View(db.Order.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

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
                        if (item.Count < item.Produkt.AntalILager)
                        {
                            var orderRad = new OrderRad
                            {

                                ProduktID = item.ProduktID,
                                OrderID = order.OrderID,
                                TotalPris = item.Produkt.Pris * item.Count,
                                Antal = item.Count
                            };

                            // Set the order total of the shopping cart
                            order.Total += orderRad.TotalPris;
                            order.OrderRader.Add(orderRad);
                        }
                    }
                    db.Order.Add(order);
                    order.OrderRader.Count();

                    db.SaveChanges();
                    return RedirectToAction("Completed", "Order", new { id = order.OrderID });
                }
            }

            return View(order);
        }


        public ActionResult Completed(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
