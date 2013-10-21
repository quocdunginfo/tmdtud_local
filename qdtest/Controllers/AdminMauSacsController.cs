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
            this.timkiem_mausac["max_item_per_page"] = "5";
        }
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._permission.Contains("mausac_view"))
            {
                return this._fail_permission("mausac_view");
            }
            //calculate offset limit
            int max_item_per_page = TextLibrary.ToInt(this.timkiem_mausac["max_item_per_page"]);
            Debug.WriteLine("max_item_per_page" + page);
            int start_point = (page - 1) * max_item_per_page;
            if (start_point <= 0) start_point = 0;
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            MauSacController ctr = new MauSacController();
            ViewBag.mausac_List = ctr.timkiem(
                timkiem_mausac["id"],
                timkiem_mausac["giatri"],
                timkiem_mausac["mota"],
                timkiem_mausac["active"], "id", true, start_point, max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_mausac = this.timkiem_mausac;
            ViewBag.Title += " - Quản lý";
            //pagination
            int Current_Page = page;
            int Total_Page = ctr.timkiem_count(
                timkiem_mausac["id"],
                timkiem_mausac["giatri"],
                timkiem_mausac["mota"],
                timkiem_mausac["active"]
                ) / max_item_per_page + 1;
            Boolean CanNextPage = Current_Page >= Total_Page ? false : true;
            Boolean CanPrevPage = Current_Page == 1 ? false : true;
            ViewBag.CanNextPage = CanNextPage;
            ViewBag.CanPrevPage = CanPrevPage;
            ViewBag.Current_Page = Current_Page;
            ViewBag.Total_page = Total_Page;

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
                this.timkiem_mausac["max_item_per_page"] = TextLibrary.ToString(Request["mausac_max_item_per_page"]);
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
                this.timkiem_mausac = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_mausac"));
            }

            //set active tab
            this._set_activetab(new String[] { "MauSac", "Catalog" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("mausac_add"))
            {
                return this._fail_permission("mausac_add");
            }
            return RedirectToAction("Add", "AdminMauSac");
        }
        public ActionResult Edit(int id = 0)
        {
            //check
            if (!this._permission.Contains("mausac_edit"))
            {
                return this._fail_permission("mausac_edit");
            }
            return RedirectToAction("Index", "AdminMauSac", new { id = id });
        }

    }
}
