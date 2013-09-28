using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using System.Web.Security;

namespace qdtest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "WTF";
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
