﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab1_WOMU.Models;
using Lab1_WOMU.Models.ViewModels;

namespace Lab1_WOMU.Controllers
{
    public class ProduktController : Controller
    {
        private DatabaseTest1 db = new DatabaseTest1();

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
        public ActionResult BuyPage(int? id)
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


        public ActionResult OrderKoll(string SearchString)
        {
            var order = db.OrderRad.Include(c => c.Produkt);

            int temp = 0;

            if ((!String.IsNullOrEmpty(SearchString)) && int.TryParse(SearchString, out temp))
            {
                order = order.Where(s => s.Order.OrderID.Equals(temp));
            } else
                order = order.Where(s => s.Order.OrderID.Equals(0));
            return View(order.ToList());
        }



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
