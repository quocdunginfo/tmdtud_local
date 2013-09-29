using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminUsersController : AdminController
    {
        //
        // GET: /AdminUsers/
        public ActionResult Index()
        {
            //check
            if (!this._permission.Contains("user_view"))
            {
                return this._fail_permission("user_view");
            }
            //this.define_active_tab();
            List<String> ActiveTab = new List<String>();
            ActiveTab.Add("NhanVien");
            ActiveTab.Add("QuanTriHeThong");
            ViewBag.ActiveTab = ActiveTab;

            ViewBag.User_List = this._db.ds_nhanvien.ToList();
            //return View(this._db.Users.ToList());
            //this._build_common_data();
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit(int id=0)
        {
            return RedirectToAction("Index", "AdminUser", new { id = id });
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Users mamagement";
        }

    }
}
