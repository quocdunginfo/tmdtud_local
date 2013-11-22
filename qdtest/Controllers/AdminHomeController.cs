using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminHomeController : AdminController
    {
        //
        // GET: /AdminHome/
        public ActionResult Index()
        {
            if (!this._nhanvien_permission.Contains("home_view"))
            {
                return this._fail_permission("home_view");
            }
            ViewBag.Title += " - Bảng điều khiển chính";
            this._set_activetab(new string[] {"Home"});
            return View();
        }
    }
}
