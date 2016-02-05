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
        private DatabaseTest1 db = new DatabaseTest1();
        
        // GET: Kunds
        public ActionResult Index()
        {
            return View(db.Kund.ToList());
        }
        
        /*
        // GET: Kunds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }
        */
        // GET: Kunds/Create
        public ActionResult Create()
        {
            return View();
        }

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
        /*
        // GET: Kunds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }
        */
        /*
        // POST: Kunds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KundID,FörNamn,Efternamn,PostAdress,PostNr,Ort,Epost,Telefonnummer")] Kund kund)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kund).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kund);
        }
        */
        /*
        // GET: Kunds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kund kund = db.Kund.Find(id);
            if (kund == null)
            {
                return HttpNotFound();
            }
            return View(kund);
        }
        */
        /*
        // POST: Kunds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kund kund = db.Kund.Find(id);
            db.Kund.Remove(kund);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
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
