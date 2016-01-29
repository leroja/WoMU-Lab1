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
    public class ProduktController : Controller
    {
        private DatabaseTest db = new DatabaseTest();

        // GET: Produkt
        public ActionResult Index()
        {
            return View(db.Produkter.ToList());
        }

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


        // GET: Produkt
        public ActionResult BuyPage()
        {
            return View();
        }


        public ActionResult OrderKoll(string SearchString)
        {
            var order = from m in db.Produkter
                        select m;

            //int temp;
            //if (!String.IsNullOrEmpty(SearchString) && int.TryParse(SearchString, out temp))
            //{
            //    order = order.Where(s => s.ProduktID.Equals(SearchString));
            //}

            if (!String.IsNullOrEmpty(SearchString))
            {
                order = order.Where(s => s.ProduktNamn.Contains(SearchString));
            }


            return View(order);
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
