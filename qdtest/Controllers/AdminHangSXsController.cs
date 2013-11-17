using qdtest._Library;
using qdtest.Controllers.ModelController;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminHangSXsController : AdminController
    {
        private HttpCookie timkiem_hangsx;
        public AdminHangSXsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_hangsx = new HttpCookie("timkiem_hangsx");
            timkiem_hangsx.Expires = DateTime.Now.AddDays(1);
            this.timkiem_hangsx["id"] = "";
            this.timkiem_hangsx["active"] = "";
            this.timkiem_hangsx["ten"] = "";
        }
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._nhanvien_permission.Contains("hangsx_view"))
            {
                return this._fail_permission("hangsx_view");
            }
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            HangSXController ctr = new HangSXController();
            ViewBag.HangSX_List = ctr.timkiem(
                timkiem_hangsx["id"],
                timkiem_hangsx["ten"],
                timkiem_hangsx["active"], "id", true, 0, -1
                );
            //set search cookies
            ViewBag.timkiem_hangsx = this.timkiem_hangsx;
            ViewBag.Title += " - Quản lý";
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get search value
            if (!TextLibrary.ToString(Request["submit_reset"]).Equals(""))
            {
                //reset button click
                this.khoitao_cookie();
            }
            else
            {
                //search button click
                this.timkiem_hangsx["id"] = TextLibrary.ToString(Request["hangsx_id"]);
                this.timkiem_hangsx["ten"] = TextLibrary.ToString(Request["hangsx_ten"]);
                this.timkiem_hangsx["active"] = TextLibrary.ToString(Request["hangsx_active"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_hangsx));
            //redirect
            return RedirectToAction("Index", "AdminHangSXs");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Hãng sản xuất";

            //build timkiem
            if (Request.Cookies.Get("timkiem_hangsx") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_hangsx));
            }
            else
            {
                try
                {
                    this.timkiem_hangsx = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_hangsx"));
                }
                catch (Exception ex)
                {
                    this.khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_hangsx));
                }
            }

            //set active tab
            this._set_activetab(new String[] { "HangSX", "Catalog" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._nhanvien_permission.Contains("hangsx_add"))
            {
                return this._fail_permission("hangsx_add");
            }
            return RedirectToAction("Add", "AdminHangSX");
        }
        public ActionResult Edit(int id = 0)
        {
            //check
            if (!this._nhanvien_permission.Contains("hangsx_edit"))
            {
                return this._fail_permission("hangsx_edit");
            }
            return RedirectToAction("Index", "AdminHangSX", new { id = id });
        }

    }
}
