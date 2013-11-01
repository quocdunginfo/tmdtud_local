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
    public class AdminLoaiNhanViensController : AdminController
    {
        private HttpCookie timkiem_loainhanvien;
        public AdminLoaiNhanViensController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_loainhanvien = new HttpCookie("timkiem_loainhanvien");
            timkiem_loainhanvien.Expires = DateTime.Now.AddDays(1);
            this.timkiem_loainhanvien["id"] = "";
            this.timkiem_loainhanvien["ten"] = "";
            this.timkiem_loainhanvien["max_item_per_page"] = "5";
        }
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._permission.Contains("loainhanvien_view"))
            {
                return this._fail_permission("loainhanvien_view");
            }
            //calculate offset limit
            int max_item_per_page = TextLibrary.ToInt(this.timkiem_loainhanvien["max_item_per_page"]);
            Debug.WriteLine("max_item_per_page" + page);
            int start_point = (page - 1) * max_item_per_page;
            if (start_point <= 0) start_point = 0;
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            LoaiNhanVienController ctr = new LoaiNhanVienController();
            ViewBag.LoaiNhanVien_List = ctr.timkiem(
                timkiem_loainhanvien["id"],
                timkiem_loainhanvien["ten"],
                "id", true, start_point, max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_loainhanvien = this.timkiem_loainhanvien;
            ViewBag.Title += " - Quản lý";
            //pagination
            int Current_Page = page;
            int Total_Page = ctr.timkiem_count(
                timkiem_loainhanvien["id"],
                timkiem_loainhanvien["ten"]
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
                this.timkiem_loainhanvien["id"] = TextLibrary.ToString(Request["loainhanvien_id"]);
                this.timkiem_loainhanvien["ten"] = TextLibrary.ToString(Request["loainhanvien_ten"]);
                this.timkiem_loainhanvien["max_item_per_page"] = TextLibrary.ToString(Request["loainhanvien_max_item_per_page"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_loainhanvien));
            //redirect
            return RedirectToAction("Index", "AdminLoaiNhanViens");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Nhóm quyền hạn";

            //build timkiem
            if (Request.Cookies.Get("timkiem_loainhanvien") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_loainhanvien));
            }
            else
            {
                this.timkiem_loainhanvien = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_loainhanvien"));
            }

            //set active tab
            this._set_activetab(new String[] { "LoaiNhanVien", "QuanTriHeThong" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("loainhanvien_add"))
            {
                return this._fail_permission("loainhanvien_add");
            }
            return RedirectToAction("Add", "AdminLoaiNhanVien");
        }
        public ActionResult Edit(int id = 0)
        {
            //check
            if (!this._permission.Contains("loainhanvien_edit"))
            {
                return this._fail_permission("loainhanvien_edit");
            }
            return RedirectToAction("Index", "AdminLoaiNhanVien", new { id = id });
        }

    }
}
