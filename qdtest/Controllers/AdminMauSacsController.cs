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
    public class AdminMauSacsController : AdminController
    {
        private HttpCookie timkiem_mausac;
        public AdminMauSacsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_mausac = new HttpCookie("timkiem_mausac");
            timkiem_mausac.Expires = DateTime.Now.AddDays(1);
            this.timkiem_mausac["id"] = "";
            this.timkiem_mausac["active"] = "";
            this.timkiem_mausac["giatri"] = "";
            this.timkiem_mausac["mota"] = "";
        }
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._nhanvien_permission.Contains("mausac_view"))
            {
                return this._fail_permission("mausac_view");
            }
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            MauSacController ctr = new MauSacController();
            ViewBag.mausac_List = ctr.timkiem(
                timkiem_mausac["id"],
                timkiem_mausac["giatri"],
                timkiem_mausac["mota"],
                timkiem_mausac["active"], "id", true, 0, -1
                );
            //set search cookies
            ViewBag.timkiem_mausac = this.timkiem_mausac;
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
                this.timkiem_mausac["id"] = TextLibrary.ToString(Request["mausac_id"]);
                this.timkiem_mausac["giatri"] = TextLibrary.ToString(Request["mausac_giatri"]);
                this.timkiem_mausac["mota"] = TextLibrary.ToString(Request["mausac_mota"]);
                this.timkiem_mausac["active"] = TextLibrary.ToString(Request["mausac_active"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_mausac));
            //redirect
            return RedirectToAction("Index", "AdminMauSacs");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Màu sắc";

            //build timkiem
            if (Request.Cookies.Get("timkiem_mausac") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_mausac));
            }
            else
            {
                try{
                    this.timkiem_mausac = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_mausac"));
                }catch(Exception)
                {
                    this.khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_mausac));
                }
            }

            //set active tab
            this._set_activetab(new String[] { "MauSac", "Catalog" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._nhanvien_permission.Contains("mausac_add"))
            {
                return this._fail_permission("mausac_add");
            }
            return RedirectToAction("Add", "AdminMauSac");
        }
        public ActionResult Edit(int id = 0)
        {
            //check
            if (!this._nhanvien_permission.Contains("mausac_edit"))
            {
                return this._fail_permission("mausac_edit");
            }
            return RedirectToAction("Index", "AdminMauSac", new { id = id });
        }
        public ActionResult Delete(int id = 0)
        {
            //check
            if (!this._nhanvien_permission.Contains("mausac_delete"))
            {
                return this._fail_permission("mausac_delete");
            }
            MauSacController controller = new MauSacController();
            if (!controller.is_exist(id))
            {
                return RedirectToAction("Index", "AdminMauSacs");
            }
            try
            {
                controller.delete(id);
            }
            catch (Exception)
            {
                return _show_notification("Màu sắc này có dính khóa ngoại với sản phẩm hiện có nên không xóa được");
            }
            return RedirectToAction("Index", "AdminMauSacs");
        }
    }
}
