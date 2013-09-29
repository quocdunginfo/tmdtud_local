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

            NhanVien u = new NhanVienController().get_by_id(id);

            if (u == null)
            {
                //user khong ton tai
                return RedirectToAction("Index", "AdminUsers");
            }
            ViewBag.NhanVien = u;
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("user_add"))
            {
                return _fail_permission("user_add");
            }
            NhanVien nv = new NhanVien();

            ViewBag.NhanVien = nv;
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - User View";
        }


    }
}
