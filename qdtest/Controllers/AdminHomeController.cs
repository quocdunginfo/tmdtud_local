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
            Debug.WriteLine("Kiểm tra home_view");
            if (!this._permission.Contains("home_view"))
            {
                return this._fail_permission("home_view");
            }
            return View();
        }
        //
        // GET: /AdminHome/

        public ActionResult Test_Login()
        {
            return View();
        }
    }
}
