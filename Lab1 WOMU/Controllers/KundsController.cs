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
    public class KundsController : Controller
    {
        private TankDatabase db = new TankDatabase();
        
        
       
        // GET: Kunds/Create
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// saves the customer and the redirects to order create
        /// </summary>
        /// <param name="kund">
        /// 
        /// </param>
        /// <returns>
        /// redirects to order create
        /// </returns>
        // POST: Kunds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KundID,FörNamn,Efternamn,PostAdress,PostNr,Ort,Epost,Telefonnummer")] Kund kund)
        {
            if (ModelState.IsValid)
            {
                db.Kund.Add(kund);
                db.SaveChanges();
                return RedirectToAction("Create", "Order");
            }

            return View(kund);
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
