using Lab1_WOMU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1_WOMU.Controllers
{
    public class HomeController : Controller
    {
        //private DatabaseTest db = new DatabaseTest();
        // GET: Home
        public ActionResult Index()
        {


            //var order = from m in db.Order
            //             select m;

            //int temp;
            //if (!String.IsNullOrEmpty(ID) && int.TryParse(ID,out temp))
            //{
            //    order = order.Where(s => s.OrderID.Equals(ID));
            //}

            //return View(order);
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


        //public ActionResult OrderKoll(string id)
        //{
        //    //var order = from m in db.Movies select m;

        //    //if (!String.IsNullOrEmpty(id))
        //    //{
        //    //    order = order.Where(s => s.Title.Contains(searchString));
        //    //}

        //    //return View(order);
        //    return View();
        //}
    }
}