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
    }
}