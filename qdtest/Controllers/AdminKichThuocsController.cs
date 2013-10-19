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
    public class AdminKichThuocsController : AdminController
    {
        private HttpCookie timkiem_kichthuoc;
        public AdminKichThuocsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_kichthuoc = new HttpCookie("timkiem_kichthuoc");
            timkiem_kichthuoc.Expires = DateTime.Now.AddDays(1);
            this.timkiem_kichthuoc["id"] = "";
            this.timkiem_kichthuoc["active"] = "";
            this.timkiem_kichthuoc["giatri"] = "";
            this.timkiem_kichthuoc["mota"] = "";
            this.timkiem_kichthuoc["max_item_per_page"] = "5";
        }
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._permission.Contains("kichthuoc_view"))
            {
                return this._fail_permission("kichthuoc_view");
            }
            //calculate offset limit
            int max_item_per_page = TextLibrary.ToInt(this.timkiem_kichthuoc["max_item_per_page"]);
            Debug.WriteLine("max_item_per_page" + page);
            int start_point = (page - 1) * max_item_per_page;
            if (start_point <= 0) start_point = 0;
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            KichThuocController ctr = new KichThuocController();
            ViewBag.KichThuoc_List = ctr.timkiem(
                timkiem_kichthuoc["id"],
                timkiem_kichthuoc["giatri"],
                timkiem_kichthuoc["mota"],
                timkiem_kichthuoc["active"], "id", true, start_point, max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_kichthuoc = this.timkiem_kichthuoc;
            ViewBag.Title += " - Quản lý";
            //pagination
            int Current_Page = page;
            int Total_Page = ctr.timkiem_count(
                timkiem_kichthuoc["id"],
                timkiem_kichthuoc["giatri"],
                timkiem_kichthuoc["mota"],
                timkiem_kichthuoc["active"]
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
                this.timkiem_kichthuoc["id"] = TextLibrary.ToString(Request["kichthuoc_id"]);
                this.timkiem_kichthuoc["giatri"] = TextLibrary.ToString(Request["kichthuoc_giatri"]);
                this.timkiem_kichthuoc["mota"] = TextLibrary.ToString(Request["kichthuoc_mota"]);
                this.timkiem_kichthuoc["active"] = TextLibrary.ToString(Request["kichthuoc_active"]);
                this.timkiem_kichthuoc["max_item_per_page"] = TextLibrary.ToString(Request["kichthuoc_max_item_per_page"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_kichthuoc));
            //redirect
            return RedirectToAction("Index", "AdminKichThuocs");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Kích thước";

            //build timkiem
            if (Request.Cookies.Get("timkiem_kichthuoc") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_kichthuoc));
            }
            else
            {
                this.timkiem_kichthuoc = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_kichthuoc"));
            }

            //set active tab
            this._set_activetab(new String[] { "KichThuoc", "Catalog" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("kichthuoc_add"))
            {
                return this._fail_permission("kichthuoc_add");
            }
            return RedirectToAction("Add", "AdminKichThuoc");
        }
        public ActionResult Edit(int id = 0)
        {
            //check
            if (!this._permission.Contains("kichthuoc_edit"))
            {
                return this._fail_permission("kichthuoc_edit");
            }
            return RedirectToAction("Index", "AdminKichThuoc", new { id = id });
        }

    }
}
