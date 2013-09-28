using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminUserController : AdminController
    {
        //
        // GET: /AdminUser/

        public ActionResult Index(int id=0)
        {
            if (!this._permission.Contains("user_view"))
            {
                return _fail_permission("user_view");
            }

            UserModel u = (from user in this._db.Users
                           where user.id == id
                           select user).FirstOrDefault();

            if (u == null)
            {
                //user khong ton tai
                return RedirectToAction("Index", "AdminUsers");
            }
            ViewBag.User = u;
            return View();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - User View";
        }

    }
}
