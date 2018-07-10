using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sunhotels Products Providers.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "xisket@gmail.com";

            return View();
        }
    }
}