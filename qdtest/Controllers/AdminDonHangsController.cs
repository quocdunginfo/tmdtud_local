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
    public class AdminDonHangsController : AdminController
    {
        private HttpCookie timkiem_donhang;
        public AdminDonHangsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_donhang = new HttpCookie("timkiem_donhang");
            timkiem_donhang.Expires = DateTime.Now.AddDays(1);
            this.timkiem_donhang["id"] = "";
            this.timkiem_donhang["khachhang_ten"] = "";
            this.timkiem_donhang["active"] = "1";
            this.timkiem_donhang["trangthai"] = "chualienlac";
            this.timkiem_donhang["tongtien_from"] = "0";
            this.timkiem_donhang["tongtien_to"] = "0";
            this.timkiem_donhang["ngay_from"] = "";
            this.timkiem_donhang["ngay_to"] = "";
            this.timkiem_donhang["max_item_per_page"] = "5";
        }
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            if (!this._nhanvien_permission.Contains("donhang_view"))
            {
                return _fail_permission("donhang_view");
            }
            
            DonHangController ctr=new DonHangController();
            //
            Boolean timkiem_ngay_from;
            Boolean timkiem_ngay_to;
            DateTime ngay_from = TextLibrary.ToDateTime(timkiem_donhang["ngay_from"], out timkiem_ngay_from);
            DateTime ngay_to = TextLibrary.ToDateTime(timkiem_donhang["ngay_to"], out timkiem_ngay_to);
            Boolean timkiem_ngay = timkiem_ngay_from && timkiem_ngay_to;
            //Pagination
                Pagination pg = new Pagination();
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_donhang["max_item_per_page"]);//get from setting
                pg.set_current_page(page);
                pg.set_max_item_per_page(max_item_per_page);
                pg.set_total_item(
                    ctr.timkiem_count(
                        timkiem_donhang["id"],
                        timkiem_donhang["khachhang_ten"],
                        TextLibrary.ToInt(timkiem_donhang["tongtien_from"]),
                        TextLibrary.ToInt(timkiem_donhang["tongtien_to"]),
                        timkiem_ngay,
                        ngay_from,
                        ngay_to,
                        timkiem_donhang["trangthai"],
                        timkiem_donhang["active"]
                        )
                    );
                pg.update();

            ViewBag.pagination = pg;
            ViewBag.DonHang_List = ctr.timkiem(
                timkiem_donhang["id"],
                timkiem_donhang["khachhang_ten"],
                TextLibrary.ToInt(timkiem_donhang["tongtien_from"]),
                TextLibrary.ToInt(timkiem_donhang["tongtien_to"]),
                timkiem_ngay,
                ngay_from,
                ngay_to,
                timkiem_donhang["trangthai"],
                timkiem_donhang["active"],
                "id",true,pg.start_point,pg.max_item_per_page
                );
            ViewBag.timkiem_donhang = timkiem_donhang;
            //pagination

            this._set_activetab(new String[] { "QuanLyDonHang","DonHang_"+timkiem_donhang["trangthai"] });
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
                this.timkiem_donhang["id"] = TextLibrary.ToString(Request["donhang_id"]);
                this.timkiem_donhang["khachhang_ten"] = TextLibrary.ToString(Request["donhang_khachhang_ten"]);
                this.timkiem_donhang["active"] = TextLibrary.ToString(Request["donhang_active"]);
                this.timkiem_donhang["trangthai"] = TextLibrary.ToString(Request["donhang_trangthai"]);
                this.timkiem_donhang["tongtien_from"] = TextLibrary.ToInt(Request["donhang_tongtien_from"]).ToString();
                this.timkiem_donhang["tongtien_to"] = TextLibrary.ToInt(Request["donhang_tongtien_to"]).ToString();
                this.timkiem_donhang["ngay_from"] = TextLibrary.ToString(Request["donhang_ngay_from"]);
                this.timkiem_donhang["ngay_to"] = TextLibrary.ToString(Request["donhang_ngay_to"]);
                this.timkiem_donhang["max_item_per_page"] = TextLibrary.ToString(Request["donhang_max_item_per_page"]);
                //this.timkiem_donhang["diachi"] = TextLibrary.ToString(Request["donhang_diachi"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
            //redirect
            return RedirectToAction("Index", "AdminDonHangs");
        }
        [HttpGet]
        public ActionResult Index_All()
        {
            this.khoitao_cookie();
            this.timkiem_donhang["trangthai"] = "chualienlac";
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
            return RedirectToAction("Index", "AdminDonHangs");
        }
        [HttpGet]
        public ActionResult Index_TrangThai(string trangthai="chualienlac")
        {
            this.khoitao_cookie();
            this.timkiem_donhang["trangthai"] = trangthai;
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
            return RedirectToAction("Index","AdminDonHangs");
        }
        [HttpGet]
        public ActionResult Index_NgayTruoc(int songay=0)
        {
            this.khoitao_cookie();
            this.timkiem_donhang["trangthai"] = "";
            //lay ngay hom nay
            DateTime tmp = DateTime.Now;
            this.timkiem_donhang["ngay_from"] = tmp.AddDays(songay).ToString("dd/MM/yyyy");
            this.timkiem_donhang["ngay_to"] = tmp.ToString("dd/MM/yyyy");
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
            return RedirectToAction("Index", "AdminDonHangs");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Đơn hàng";

            //build timkiem_nhanvien
            if (Request.Cookies.Get("timkiem_donhang") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
            }
            else
            {
                try{
                    this.timkiem_donhang = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_donhang"));
                }
                catch (Exception ex)
                {
                    this.khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_donhang));
                }
            }
            
            //set active tab
            this._set_activetab(new String[] { "DonHang" });
        }

    }
}
