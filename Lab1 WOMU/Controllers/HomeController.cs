using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1_WOMU.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult OrderKoll(string id)
        {
            //var order = from m in db.Movies select m;

            //if (!String.IsNullOrEmpty(id))
            //{
            //    order = order.Where(s => s.Title.Contains(searchString));
            //}

            //return View(order);
            return View();
        }
    }
}