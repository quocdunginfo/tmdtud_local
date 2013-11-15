using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminDonHangController : AdminController
    {
        [HttpGet]
        public ActionResult Index(int id=0)
        {
            if (!this._permission.Contains("donhang_view"))
            {
                return _fail_permission("donhang_view");
            }
            DonHangController ctr=new DonHangController();
            DonHang obj = ctr.get_by_id(id);
            if (obj == null)
            {
                return RedirectToAction("Index","AdminDonHangs");
            }
            ViewBag.donhang = obj;
            string[] state = (string[])TempData["state"];
            if (state != null)
            {
                this._state.AddRange(state);
            }
            ViewBag.State = this._state;
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            if (!this._permission.Contains("donhang_edit"))
            {
                return _fail_permission("donhang_edit");
            }
            int id = TextLibrary.ToInt(Request["donhang_id"]);
            DonHangController ctr=new DonHangController();
            DonHang obj= ctr.get_by_id(id);
            if(obj==null)
            {
                return RedirectToAction("Index","AdminDonHangs");
            }
            obj.trangthai = TextLibrary.ToString(Request["donhang_trangthai"]);
            //luu
            ctr._db.SaveChanges();
            TempData["state"] = new string[] { "edit_ok" };
            return RedirectToAction("Index", "AdminDonHang", new { id=obj.id });
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Đơn hàng - Chi Tiết";
            //set active tab
            this._set_activetab(new String[] { "DonHang" });
        }

    }
}
