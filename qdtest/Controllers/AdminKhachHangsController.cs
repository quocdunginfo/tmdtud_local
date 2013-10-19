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
    public class AdminKhachHangsController : AdminController
    {
        private HttpCookie timkiem_khachhang;
        public AdminKhachHangsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_khachhang = new HttpCookie("timkiem_khachhang");
            timkiem_khachhang.Expires = DateTime.Now.AddDays(1);
            this.timkiem_khachhang["id"] = "";
            this.timkiem_khachhang["tendangnhap"] = "";
            this.timkiem_khachhang["tendaydu"] = "";
            this.timkiem_khachhang["active"] = "";
            this.timkiem_khachhang["email"] = "";
            this.timkiem_khachhang["sdt"] = "";
            this.timkiem_khachhang["diachi"] = "";
            this.timkiem_khachhang["max_item_per_page"] = "5";
        }
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._permission.Contains("khachhang_view"))
            {
                return this._fail_permission("khachhang_view");
            }
            //calculate offset limit
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_khachhang["max_item_per_page"]);
            Debug.WriteLine("max_item_per_page"+page);
                int start_point = (page - 1) * max_item_per_page;
                if (start_point <= 0) start_point = 0;
            //Chọn danh sách nhân viên để hiển thị theo cookies tìm kiếm
            KhachHangController ctr = new KhachHangController();
            ViewBag.KhachHang_List = ctr.timkiem(
                timkiem_khachhang["id"],
                timkiem_khachhang["tendangnhap"],
                timkiem_khachhang["tendaydu"],
                timkiem_khachhang["email"],
                timkiem_khachhang["sdt"],
                timkiem_khachhang["diachi"],
                timkiem_khachhang["active"],"id",true,start_point,max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_khachhang = this.timkiem_khachhang;
            ViewBag.Title += " - Quản lý";
            //pagination
                int Current_Page = page;
                int Total_Page = ctr.timkiem_count(
                    timkiem_khachhang["id"],
                    timkiem_khachhang["tendangnhap"],
                    timkiem_khachhang["tendaydu"],
                    timkiem_khachhang["email"],
                    timkiem_khachhang["sdt"],
                    timkiem_khachhang["diachi"],
                    timkiem_khachhang["active"]
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
                this.timkiem_khachhang["id"] = TextLibrary.ToString(Request["khachhang_id"]);
                this.timkiem_khachhang["tendangnhap"] = TextLibrary.ToString(Request["khachhang_tendangnhap"]);
                this.timkiem_khachhang["tendaydu"] = TextLibrary.ToString(Request["khachhang_tendaydu"]);
                this.timkiem_khachhang["email"] = TextLibrary.ToString(Request["khachhang_email"]);
                this.timkiem_khachhang["sdt"] = TextLibrary.ToString(Request["khachhang_sdt"]);
                this.timkiem_khachhang["active"] = TextLibrary.ToString(Request["khachhang_active"]);
                this.timkiem_khachhang["max_item_per_page"] = TextLibrary.ToString(Request["khachhang_max_item_per_page"]);
                //this.timkiem_khachhang["diachi"] = TextLibrary.ToString(Request["khachhang_diachi"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_khachhang));
            //redirect
            return RedirectToAction("Index", "AdminKhachHangs");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Khách hàng";

            //build timkiem_nhanvien
            if (Request.Cookies.Get("timkiem_khachhang") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_khachhang));
            }
            else
            {
                this.timkiem_khachhang = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_khachhang"));
            }
            
            //set active tab
            this._set_activetab(new String[] { "KhachHang" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("khachhang_add"))
            {
                return this._fail_permission("khachhang_add");
            }
            return RedirectToAction("Add", "AdminKhachHang");
        }
        public ActionResult Edit(int id=0)
        {
            //check
            if (!this._permission.Contains("khachhang_edit"))
            {
                return this._fail_permission("khachhang_edit");
            }
            return RedirectToAction("Index", "AdminKhachHang", new { id=id});
        }
    }
}
