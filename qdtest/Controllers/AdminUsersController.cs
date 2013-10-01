using qdtest.Controllers.ModelController;
using qdtest.Models;
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
            this._set_activetab(new String[] {"NhanVien","QuanTriHeThong"});

            ViewBag.User_List = this._db.ds_nhanvien.ToList();
            //return View(this._db.Users.ToList());
            //this._build_common_data();
            ViewBag.Title += " - Management";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("user_add"))
            {
                return _fail_permission("user_add");
            }
            //ViewBag.Title += " - Add";
            return RedirectToAction("Add","AdminUser");
        }
        public ActionResult Edit(int id=0)
        {
            if (!this._permission.Contains("user_edit"))
            {
                return _fail_permission("user_edit");
            }
            return RedirectToAction("Index", "AdminUser", new { id = id });
        }
        public ActionResult Delete(int id=0)
        {
            if (!this._permission.Contains("user_delete"))
            {
                return _fail_permission("user_delete");
            }
            new NhanVienController().delete(id);
            return RedirectToAction("Index","AdminUsers");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Users";
        }

    }
}
