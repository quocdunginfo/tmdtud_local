using qdtest._Library;
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
            if (!this._nhanvien_permission.Contains("khachhang_view"))
            {
                return this._fail_permission("khachhang_view");
            }
            KhachHangController ctr = new KhachHangController();
            //pagination
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_khachhang["max_item_per_page"]);//get from setting
                Pagination pg = new Pagination();
                pg.set_current_page(page);
                pg.set_max_item_per_page(max_item_per_page);
                pg.set_total_item(
                    ctr.timkiem_count(
                        timkiem_khachhang["id"],
                        timkiem_khachhang["tendangnhap"],
                        timkiem_khachhang["tendaydu"],
                        timkiem_khachhang["email"],
                        timkiem_khachhang["sdt"],
                        timkiem_khachhang["diachi"],
                        timkiem_khachhang["active"]
                        )
                    );
                pg.update();
            //Chọn danh sách nhân viên để hiển thị theo cookies tìm kiếm
            ViewBag.KhachHang_List = ctr.timkiem(
                timkiem_khachhang["id"],
                timkiem_khachhang["tendangnhap"],
                timkiem_khachhang["tendaydu"],
                timkiem_khachhang["email"],
                timkiem_khachhang["sdt"],
                timkiem_khachhang["diachi"],
                "",
                timkiem_khachhang["active"], "id", true, pg.start_point, pg.max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_khachhang = this.timkiem_khachhang;
            ViewBag.Title += " - Quản lý";
            ViewBag.pagination = pg;
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
            this._set_activetab(new String[] { "KhachHang", "QuanLyKhachHang" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._nhanvien_permission.Contains("khachhang_add"))
            {
                return this._fail_permission("khachhang_add");
            }
            return RedirectToAction("Add", "AdminKhachHang");
        }
        public ActionResult Edit(int id=0)
        {
            //check
            if (!this._nhanvien_permission.Contains("khachhang_edit"))
            {
                return this._fail_permission("khachhang_edit");
            }
            return RedirectToAction("Index", "AdminKhachHang", new { id=id});
        }
        public ActionResult Delete(int id = 0)
        {
            //check
            if (!this._nhanvien_permission.Contains("khachhang_delete"))
            {
                return this._fail_permission("khachhang_delete");
            }

            KhachHangController controller = new KhachHangController();
            if (!controller.is_exist(id))
            {
                return RedirectToAction("Index", "AdminKhachHangs");
            }
            try
            {
                controller.delete(id);
            }
            catch (Exception)
            {
                return _show_notification("Khách hàng này có dính khóa ngoại với đơn hàng hiện có nên không xóa được");
            }
            return RedirectToAction("Index", "AdminKhachHangs");
        }
    }
}
