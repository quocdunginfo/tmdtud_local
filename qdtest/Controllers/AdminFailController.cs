using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminFailController : AdminController
    {
        //
        // GET: /AdminFail/

        public ActionResult Index()
        {
            ViewBag.Message = TempData["admin_fail_message"];
            ViewBag.Title += " - Fail Permission";
            return View();
        }

    }
}
